using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaCañon : MonoBehaviour
{
    public int damage;
    private float spawnTime;
    public float lifeTime;
    private Rigidbody2D rig;
    public float direction;
    public float speed;
    // Start is called before the first frame update
     private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("CollisionLayer: " + collision.gameObject.layer);
        if (collision!=null && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().HurtPlayer(damage,transform.position,true,false);
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;
    }
    private void Update()
    {
        if (Time.time > spawnTime+lifeTime)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        GoForward();
    }
    private void GoForward()
    {
        rig.velocity = new Vector2(rig.velocity.x+speed*direction,rig.velocity.y);   
    }
}
