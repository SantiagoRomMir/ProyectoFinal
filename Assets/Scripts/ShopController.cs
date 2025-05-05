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
                if (!isShooping && collision.gameObject.GetComponent<PlayerController>().money >= price)
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
        int rand = Random.Range(0, consumables.Count);
        consumables[rand].remainingAmount--;
        if (consumables[rand].remainingAmount<=0)
        {
            consumables.Remove(consumables[rand]);
        }

        return consumables[rand];
    }
    IEnumerator GiveConsumablePlayer()
    {
        Consumable c = GetRandomConsumable();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().money -= price;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().AddConsumable(c);
        yield return new WaitForSeconds(shopDelay);
        isShooping = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().enabled = true;
    }
}
