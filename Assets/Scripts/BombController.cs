using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    private float spawnTime;
    public float lifeTime;
    public int damage;
    void Awake()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= spawnTime+lifeTime)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision!=null && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().HurtPlayer(damage,transform.position,false,false);
        }
    }
}
