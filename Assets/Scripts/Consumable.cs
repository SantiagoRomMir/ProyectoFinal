using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public class Consumable
{
    public enum TypeConsumable
    {
        Datil,
        Ron,
        Pera,
        Hierbabuena,
        Money
    }
    public TypeConsumable consumable;
    public int numRon;
    private float effectDuration;
    public int remainingAmount;
    GameObject player;
    public Sprite spriteDatil, spritePera, spriteHierbabuena;
    public Consumable(ConsumableController c)
    {
        remainingAmount = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        consumable = c.consumable;
    }
    public Consumable(Consumable c)
    {
        remainingAmount = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        consumable = c.consumable;
    }
    public Consumable(int remainingAmount, TypeConsumable consumable)
    {
        this.remainingAmount = remainingAmount;
        player = GameObject.FindGameObjectWithTag("Player");
        this.consumable = consumable;
    }
    public void OnUseAction()
    {
        if (remainingAmount<=0)
        {
            return;
        }
        //Debug.Log("Used: " + consumable);

        switch (consumable)
        {
            case TypeConsumable.Datil:
                effectDuration = 10f;
                player.GetComponent<PlayerController>().StartDamageBuff(effectDuration);
                break;
            case TypeConsumable.Pera:
                effectDuration = 5f;
                player.GetComponent<PlayerController>().StartHpRegen(effectDuration);
                break;
            case TypeConsumable.Hierbabuena:
                effectDuration = 7f;
                player.GetComponent<PlayerController>().StartDefenseBuff(effectDuration);
                break;
        }

        remainingAmount--;
    }
    public static TypeConsumable GetConsumableTypeByName(string name)
    {
        return (TypeConsumable)Enum.Parse(typeof(TypeConsumable), name);
    }
}
