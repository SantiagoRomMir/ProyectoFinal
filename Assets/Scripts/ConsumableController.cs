using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableController : MonoBehaviour
{
    public Consumable.TypeConsumable consumable;
    public int numRon;
    public int money;
    private float speedDifference;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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
    private void Awake()
    {
        if (consumable.Equals(Consumable.TypeConsumable.Money))
        {
            transform.localScale = new Vector2(transform.localScale.x - (5 - money) * 0.05f, transform.localScale.y - (5 - money) * 0.05f);
            speedDifference = (float)money/5 * 2f;
            Debug.Log(speedDifference);
        }
    }
    private void FixedUpdate()
    {
        if (consumable.Equals(Consumable.TypeConsumable.Money))
        {
            Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, Time.deltaTime * (2.5f - speedDifference + 1 * Vector2.Distance(transform.position, playerPos)*2f));
        }
    }
}