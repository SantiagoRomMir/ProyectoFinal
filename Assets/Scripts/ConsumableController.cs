using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableController : MonoBehaviour
{
    public Consumable.TypeConsumable consumable;
    public int numRon;
    public int money;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            switch (consumable)
            {
                case Consumable.TypeConsumable.Ron:
                    collision.GetComponent<PlayerController>().ReplenishRon(numRon);
                    Destroy(gameObject);
                    break;
                case Consumable.TypeConsumable.Money:
                    collision.GetComponent<PlayerController>().AddMoney(money);
                    Destroy(gameObject);
                    break;
                default:
                    collision.GetComponent<PlayerController>().AddConsumable(this);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
