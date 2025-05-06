using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopController : MonoBehaviour
{
    public List<Consumable> consumables;
    public float shopDelay;
    private bool isShooping;
    public int price;
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (!isShooping && collision.gameObject.GetComponent<PlayerController>().money >= price && consumables.Count>0)
                {
                    isShooping = true;
                    StartCoroutine("GiveConsumablePlayer");
                }
            }
        }
    }
    private Consumable GetRandomConsumable()
    {
        Debug.Log("ListCount: "+consumables.Count);
        int rand = UnityEngine.Random.Range(0, consumables.Count);
        try
        {
            consumables[rand].remainingAmount--;
            Consumable tmp = consumables[rand];
            if (consumables[rand].remainingAmount <= 0)
            {
                consumables.Remove(consumables[rand]);
            }
            return tmp;

        }
        catch (ArgumentOutOfRangeException e)
        {
            Debug.Log("ErrorCount: " + consumables.Count+" | Index: "+rand+" Exception: "+e.ToString());
            return null;
        }
    }
    IEnumerator GiveConsumablePlayer()
    {
        Debug.Log("ItemBought");
        Consumable c = GetRandomConsumable();
        if (c!=null)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().money -= price;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().AddConsumable(c);
        }
        yield return new WaitForSeconds(shopDelay);
        isShooping = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().enabled = true;
    }
}
