using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Consumable
{
    public enum TypeConsumable
    {
        Datil,
        Ron,
        LifeRegen,
        DefenseBuff
    }
    public TypeConsumable consumable;
    public int numRon;
    private float effectDuration;
    public int remainingAmount;
    GameObject player;
    public Consumable(ConsumableController c)
    {
        remainingAmount = 1;
        player = GameObject.FindGameObjectWithTag("Player");
        consumable = c.consumable;
    }

    public void OnUseAction()
    {
        Debug.Log("Used: " + consumable);

        switch (consumable)
        {
            case TypeConsumable.Datil:
                effectDuration = 10f;
                player.GetComponent<PlayerController>().StartDamageBuff(effectDuration);
                break;
            case TypeConsumable.LifeRegen:
                effectDuration = 5f;
                player.GetComponent<PlayerController>().StartHpRegen(effectDuration);
                break;
            case TypeConsumable.DefenseBuff:
                effectDuration = 7f;
                player.GetComponent<PlayerController>().StartDefenseBuff(effectDuration);
                break;
        }

        remainingAmount--;

        if (remainingAmount <= 0)
        {
            player.GetComponent<PlayerController>().RemoveConsumable(this);
        }
    }
}
