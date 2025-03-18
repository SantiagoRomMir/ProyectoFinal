using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPoint : MonoBehaviour
{
    private PlayerController player;
    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            player.ganchos.Add(transform);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){

            player.ganchos.Remove(transform);
        }
    }
}
