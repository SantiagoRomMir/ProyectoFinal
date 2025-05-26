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
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ChangeInventoryIcons(Consumable selected, Consumable left, Consumable right)
    {
        selectedItem.sprite = GetSpriteConsumable(selected.consumable);
        leftItem.sprite = GetSpriteConsumable(left.consumable);
        rightItem.sprite = GetSpriteConsumable(right.consumable);
        selectedAmount.GetComponent<TMP_Text>().text = selected.remainingAmount.ToString();
        leftAmount.GetComponent<TMP_Text>().text = left.remainingAmount.ToString();
        rightAmount.GetComponent<TMP_Text>().text = right.remainingAmount.ToString();
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
        Debug.Log(porcentaje);
        playerLife.fillAmount = porcentaje;
    }
    public void UpdateInternalDamage(float porcentaje)
    {
        internalDamage.fillAmount = porcentaje;
    }
    public void UpdateBossInternalDamage(float porcentaje)
    {
        internalDamage.fillAmount = porcentaje;
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
