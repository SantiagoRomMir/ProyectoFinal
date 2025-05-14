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
        for (int i = 0; i < Random.Range(1, 6); i++)
        {
            moneyMultiplier = Random.Range(1, 4);
            money.GetComponent<ConsumableController>().money = (int)(Random.Range(1, 6) + 1 * moneyMultiplier);
            money.transform.position = transform.position;
            Instantiate(money);
        }

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
