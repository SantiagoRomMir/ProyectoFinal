using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Persistence
{
    public float hp;
    public float ron;
    public float internalDamage;
    public int selectedConsumable;
    public int addedDamage;
    public float defense;
    public bool hasHook;
    public bool hasParrot;
    public bool hasGun;
    public bool canShoot;
    public int money;

    public Persistence(int hp, int ron, int internalDamage, int selectedConsumable, int addedDamage, float defense, bool hasHook, bool hasParrot, bool hasGun, bool canShoot, int money)
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
        this.money = money;
    }
    public static Persistence LoadPersistence()
    {
        if (PlayerPrefs.GetString("hasHook").Length<2 || PlayerPrefs.GetString("hasParrot").Length<2 || PlayerPrefs.GetString("hasGun").Length<2 || PlayerPrefs.GetString("canShoot").Length<2)
        {
            Debug.Log("Missing Data Persistence, Returning Null");

            return null;
        }

        float hp = PlayerPrefs.GetFloat("hp");
        float ron = PlayerPrefs.GetFloat("ron");
        float internalDamage = PlayerPrefs.GetFloat("internalDamage");
        int selectedConsumable = PlayerPrefs.GetInt("selectedConsumable");
        int addedDamage = PlayerPrefs.GetInt("addedDamage");
        float defense = PlayerPrefs.GetFloat("defense");
        //Debug.Log(PlayerPrefs.GetString("hasHook"));
        bool hasHook = Boolean.Parse(PlayerPrefs.GetString("hasHook"));
        bool hasParrot = Boolean.Parse(PlayerPrefs.GetString("hasParrot"));
        bool hasGun = Boolean.Parse(PlayerPrefs.GetString("hasGun"));
        bool canShoot = Boolean.Parse(PlayerPrefs.GetString("canShoot"));
        int money = PlayerPrefs.GetInt("money");

        Debug.Log("PersistenceLoaded");

        return new Persistence(hp, ron, internalDamage, selectedConsumable, addedDamage, defense, hasHook, hasParrot, hasGun, canShoot, money);
    }
    public void SavePersistence() 
    {
        PlayerPrefs.SetFloat("hp", hp);
        PlayerPrefs.SetFloat("ron", ron);
        PlayerPrefs.SetFloat("internalDamage", internalDamage);
        PlayerPrefs.SetInt("selectedConsumable", selectedConsumable);
        PlayerPrefs.SetInt("addedDamage", addedDamage);
        PlayerPrefs.SetFloat("defense", defense);
        PlayerPrefs.SetString("hasHook",hasHook.ToString());
        PlayerPrefs.SetString("hasParrot", hasParrot.ToString());
        PlayerPrefs.SetString("hasGun", hasGun.ToString());
        PlayerPrefs.SetString("canShoot", canShoot.ToString());
        PlayerPrefs.SetInt("money", money);

        Debug.Log("PersistenceSaved");
    }

    public static void SavePersistenceEnemiesDead(string currentSceneName, List<int> indexes)
    {
        string values = "";
        foreach (int i in indexes)
        {
            values += i+",";
        }
        if (values.Length>0)
        {
            values = values.Substring(0, values.Length - 1);
        }
        PlayerPrefs.SetString(currentSceneName,values);
        //Debug.Log("key: "+currentSceneName+" -> "+values);
    }
    public static List<int> LoadPersistenceEnemiesDead(string currentSceneName)
    {
        List<int> values = new List<int>();
        string data = PlayerPrefs.GetString(currentSceneName);
        if (data.Length>0)
        {
            foreach (string s in data.Split(","))
            {
                //Debug.Log(s);
                values.Add(int.Parse(s));
            }
        }
        return values;
    }
    public static void SavePersistenceVasesBroken(string currentSceneName, List<int> indexes)
    {
        string values = "";
        foreach (int i in indexes)
        {
            values += i + ",";
        }
        if (values.Length > 0)
        {
            values = values.Substring(0, values.Length - 1);
        }
        PlayerPrefs.SetString(currentSceneName+"_events", values);
        //Debug.Log("key: " + currentSceneName+"_events" + " -> " + values);
    }
    public static List<int> LoadPersistenceVaseBroken(string currentSceneName)
    {
        List<int> values = new List<int>();
        string data = PlayerPrefs.GetString(currentSceneName+"_events");
        if (data.Length > 0)
        {
            foreach (string s in data.Split(","))
            {
                Debug.Log(s);
                values.Add(int.Parse(s));
            }
        }
        return values;
    }
    public static void SavePersistenceInventory(List<Consumable> consumables)
    {
        string items = "";
        foreach(Consumable c in consumables)
        {
            items += c.consumable + "," + c.remainingAmount + ":";
        }
        if (items.Length > 0)
        {
            items = items.Substring(0, items.Length - 1);
        }
        Debug.Log(items);
        PlayerPrefs.SetString("inventory",items);
    }
    public static List<Consumable> LoadPersistenceInventory()
    {
        List<Consumable> consumables = new List<Consumable>();
        string items = PlayerPrefs.GetString("inventory");
        if (items.Length > 0)
        {
            foreach (string s in items.Split(":"))
            {
                string consumableName = s.Split(",")[0];
                int amount = int.Parse(s.Split(",")[1]);
                Consumable c = new Consumable(amount, Consumable.GetConsumableTypeByName(consumableName));
                //Debug.Log(s);
                consumables.Add(c);
            }
        }
        return consumables;
    }
}
