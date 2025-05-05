using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudControl : MonoBehaviour
{
    public Image bossLife;
    public Image ron;
    public Image internalDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateBossLife(int porcentaje){
        bossLife.fillAmount=porcentaje;
    }
    public void UpdateRon(int porcentaje){
        ron.fillAmount=porcentaje;
    }
    public void UpdatePlayerLife(int porcentaje){

    }
    public void UpdateInternalDamage(int porcentaje){

    }
}
