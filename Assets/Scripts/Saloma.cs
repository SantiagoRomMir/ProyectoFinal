using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saloma : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
