using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableController : MonoBehaviour
{
    public Consumable.TypeConsumable consumable;
    public int numRon;
    public int money;
    private float speedDifference;
    private float spawnTime;
    private float speed;
    public RuntimeAnimatorController moneyAnimator;
    public Sprite spriteDatil, spriteHierbabuena, spritePera;
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
        SetSpriteConsumable(GetSpriteConsumable(consumable));
        if (consumable.Equals(Consumable.TypeConsumable.Money))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            gameObject.AddComponent<Animator>().runtimeAnimatorController = moneyAnimator;
            GetComponent<Animator>().SetInteger("MoneyAmount", money);
            GetComponent<Animator>().SetTrigger("ShowMoney");
            transform.localScale = new Vector2(transform.localScale.x - (5 - money) * 0.05f, transform.localScale.y - (5 - money) * 0.05f);
            speedDifference = (float)money/5 * 2f;
            //Debug.Log(speedDifference);
            spawnTime = Time.time;
            speed = 10f;
        }
    }
    private void SetSpriteConsumable(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
    public Sprite GetSpriteConsumable(Consumable.TypeConsumable c)
    {
        switch (c)
        {
            case Consumable.TypeConsumable.Datil:
                return spriteDatil;
            case Consumable.TypeConsumable.Pera:
                return spritePera;
            case Consumable.TypeConsumable.Hierbabuena:
                return spriteHierbabuena;
        }
        return null;
    }
    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }
    private void FixedUpdate()
    {
        if (consumable.Equals(Consumable.TypeConsumable.Money))
        {
            Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, Time.deltaTime * (speed - speedDifference + 1 * Vector2.Distance(transform.position, playerPos) * 2f));
            if (Time.time > spawnTime + 5f)
            {
                speedDifference = 0;
                speed *= 1.5f;
            }
        }
    }
}