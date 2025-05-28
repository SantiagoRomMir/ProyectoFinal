using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudControl : MonoBehaviour
{
    public Image playerLife;
    public Image bossLife;
    public Image ron;
    public Image bossInternalDamage;
    public Image internalDamage;
    public Image GunPowder;
    public GameObject blackScreen;
    [Header("Inventory")]
    public Image selectedItem;
    public Image leftItem;
    public Image rightItem;
    public GameObject selectedAmount;
    public GameObject leftAmount;
    public GameObject rightAmount;
    public Sprite spriteDatil;
    public Sprite spriteHierbabuena;
    public Sprite spritePera;
    public GameObject moneyAmount;
    public GameObject activeBuffs;
    public GameObject buffEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void UpdateActiveBuffs(int index, bool isActive)
    {
        activeBuffs.transform.GetChild(index).gameObject.SetActive(isActive);
    }
    public void ChangeInventoryIcons(Consumable selected, Consumable left, Consumable right)
    {
        selectedItem.sprite = GetSpriteConsumable(selected.consumable);
        leftItem.sprite = GetSpriteConsumable(left.consumable);
        rightItem.sprite = GetSpriteConsumable(right.consumable);
        selectedAmount.GetComponent<TMP_Text>().text = selected.remainingAmount.ToString();
        if (selected.remainingAmount==0)
        {
            selectedItem.color = new Color32(255, 255, 255, 100);
        } else
        {
            selectedItem.color = new Color32(255, 255, 255, 255);
        }
        leftAmount.GetComponent<TMP_Text>().text = left.remainingAmount.ToString();
        if (left.remainingAmount == 0)
        {
            leftItem.color = new Color32(255, 255, 255, 100);
        }
        else
        {
            leftItem.color = new Color32(255, 255, 255, 255);
        }
        rightAmount.GetComponent<TMP_Text>().text = right.remainingAmount.ToString();
        if (right.remainingAmount == 0)
        {
            rightItem.color = new Color32(255, 255, 255, 100);
        }
        else
        {
            rightItem.color = new Color32(255, 255, 255, 255);
        }
        buffEffect.GetComponent<TMP_Text>().text = GetEffectConsumable(selected.consumable);
    }
    public void UpdateMoney(int money)
    {
        moneyAmount.GetComponent<TMP_Text>().text = money.ToString();
    }
    public Sprite GetSpriteConsumable(Consumable.TypeConsumable c)
    {
        switch (c)
        {
            case Consumable.TypeConsumable.Datil:
                return spriteDatil;
            case Consumable.TypeConsumable.Pera:
                return spritePera;
            case Consumable.TypeConsumable.Hierbabuena:
                return spriteHierbabuena;
        }
        return null;
    }
    public string GetEffectConsumable(Consumable.TypeConsumable c)
    {
        switch (c)
        {
            case Consumable.TypeConsumable.Datil:
                return "Aumento Daño";
            case Consumable.TypeConsumable.Pera:
                return "Regeneración Vida";
            case Consumable.TypeConsumable.Hierbabuena:
                return "Aumento Defensa";
        }
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateBossLife(float porcentaje)
    {
        bossLife.fillAmount = porcentaje;
    }
    public void UpdateRon(float porcentaje)
    {
        ron.fillAmount = porcentaje;
    }
    public void UpdatePlayerLife(float porcentaje)
    {
        //Debug.Log(porcentaje);
        playerLife.fillAmount = porcentaje;
    }
    public void UpdateInternalDamage(float porcentaje)
    {
        internalDamage.fillAmount = porcentaje;
    }
    public void UpdateBossInternalDamage(float porcentaje)
    {
        bossInternalDamage.fillAmount = porcentaje;
    }
    public void ActiveBossBar()
    {
        bossInternalDamage.gameObject.SetActive(true);
    }
    public void ActiveGunPowder()
    {
        GunPowder.gameObject.SetActive(!GunPowder.gameObject.activeSelf);
    }
    public void FadeToBlack()
    {
        blackScreen.GetComponent<Animator>().SetTrigger("FadeToBlack");
    }
    public void FadeToAlpha()
    {
        blackScreen.GetComponent<Animator>().SetTrigger("FadeToAlpha");
    }
}
