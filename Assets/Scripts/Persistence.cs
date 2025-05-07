using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistence
{
    public int hp;
    public int ron;
    public int internalDamage;
    public int selectedConsumable;
    public int addedDamage;
    public float defense;
    public bool hasHook;
    public bool hasParrot;
    public bool hasGun;
    public bool canShoot;

    /*
     * 1- Es necesario añadir la persistencia de enemigos muertos y los objetos destruidos hasta el reinicio en un punto de descanso
     * 2- Es necesario preservar aquellos mecanismos o eventos persistentes del mapa
     * 3- Es necesario almacenar los objetos ya recolectados
     */
    public Persistence(int hp, int ron, int internalDamage, int selectedConsumable, int addedDamage, float defense, bool hasHook, bool hasParrot, bool hasGun, bool canShoot)
    {
        this.hp = hp;
        this.ron = ron;
        this.internalDamage = internalDamage;
        this.selectedConsumable = selectedConsumable;
        this.addedDamage = addedDamage;
        this.defense = defense;
        this.hasHook = hasHook;
        this.hasParrot = hasParrot;
        this.hasGun = hasGun;
        this.canShoot = canShoot;
    }
    public static Persistence LoadPersistence()
    {
        if (PlayerPrefs.GetString("hasHook").Length<2 || PlayerPrefs.GetString("hasParrot").Length<2 || PlayerPrefs.GetString("hasGun").Length<2 || PlayerPrefs.GetString("canShoot").Length<2)
        {
            Debug.Log("Missing Data Persistence, Returning Null");

            return null;
        }

        int hp = PlayerPrefs.GetInt("hp");
        int ron = PlayerPrefs.GetInt("ron");
        int internalDamage = PlayerPrefs.GetInt("internalDamage");
        int selectedConsumable = PlayerPrefs.GetInt("selectedConsumable");
        int addedDamage = PlayerPrefs.GetInt("addedDamage");
        float defense = PlayerPrefs.GetFloat("defense");
        Debug.Log(PlayerPrefs.GetString("hasHook"));
        bool hasHook = Boolean.Parse(PlayerPrefs.GetString("hasHook"));
        bool hasParrot = Boolean.Parse(PlayerPrefs.GetString("hasParrot"));
        bool hasGun = Boolean.Parse(PlayerPrefs.GetString("hasGun"));
        bool canShoot = Boolean.Parse(PlayerPrefs.GetString("canShoot"));

        Debug.Log("PersistenceLoaded");

        return new Persistence(hp, ron, internalDamage, selectedConsumable, addedDamage, defense, hasHook, hasParrot, hasGun, canShoot);
    }
    public void SavePersistence() 
    {
        PlayerPrefs.SetInt("hp", hp);
        PlayerPrefs.SetInt("ron", ron);
        PlayerPrefs.SetInt("internalDamage", internalDamage);
        PlayerPrefs.SetInt("selectedConsumable", selectedConsumable);
        PlayerPrefs.SetInt("addedDamage", addedDamage);
        PlayerPrefs.SetFloat("defense", defense);
        PlayerPrefs.SetString("hasHook",hasHook.ToString());
        PlayerPrefs.SetString("hasParrot", hasParrot.ToString());
        PlayerPrefs.SetString("hasGun", hasGun.ToString());
        PlayerPrefs.SetString("canShoot", canShoot.ToString());

        Debug.Log("PersistenceSaved");
    }
}
