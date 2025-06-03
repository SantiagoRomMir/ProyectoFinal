using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampaVertical : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector2.down * 10 * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().HurtPlayer(5, transform.position, true, false);
            }
            Destroy(gameObject);
        }
        
    }
}
