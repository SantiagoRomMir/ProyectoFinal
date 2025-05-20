using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        
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
