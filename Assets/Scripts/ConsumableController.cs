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
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (consumable)
            {
                case Consumable.TypeConsumable.Ron:
                    collision.GetComponent<PlayerController>().ReplenishRon(numRon);
                    gameObject.SetActive(false);
                    break;
                case Consumable.TypeConsumable.Money:
                    collision.GetComponent<PlayerController>().AddMoney(money);
                    Destroy(gameObject);
                    break;
                default:
                    collision.GetComponent<PlayerController>().AddConsumable(this);
                    gameObject.SetActive(false);
                    break;
            }
        }
    }
    private void Awake()
    {
        if (consumable.Equals(Consumable.TypeConsumable.Money))
        {
            transform.localScale = new Vector2(transform.localScale.x - (5 - money) * 0.05f, transform.localScale.y - (5 - money) * 0.05f);
            speedDifference = (float)money/5 * 2f;
            //Debug.Log(speedDifference);
            spawnTime = Time.time;
            speed = 5f;
        }
    }
    private void FixedUpdate()
    {
        if (consumable.Equals(Consumable.TypeConsumable.Money))
        {
            Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, Time.deltaTime * (2.5f + 1 * Vector2.Distance(transform.position, playerPos)));
        }
    }
}