using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseController : MonoBehaviour
{
    [Header("MoneyDrop")]
    public GameObject money;
    public float moneyMultiplier;
    public void DropMoney()
    {
        for (int i = 0; i < Random.Range(1, 6) * moneyMultiplier; i++)
        {
            int randMoney = (int)(Random.Range(5, 16) + 1 * moneyMultiplier);
            money.GetComponent<ConsumableController>().money = randMoney;
            money.transform.position = transform.position;
            Instantiate(money);
        }

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
