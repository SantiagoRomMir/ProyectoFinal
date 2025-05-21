using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float spawnTime;
    public float lifeTime;
    public int damage;
    private Rigidbody2D rig;
    public float direction;
    public float speed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
        //Debug.Log("CollisionLayer: " + collision.gameObject.layer);
        if (collision!=null && collision.CompareTag("Enemy"))
        {
            // No hace daño si golpea contra un piquero con su escudo en alto
            if (collision.GetComponent<EnemyController>().shield)
            {
                Debug.Log(transform.position.x - collision.transform.position.x);
                if (collision.GetComponent<SpriteRenderer>().flipX) // Izquierda
                {
                    if (transform.position.x - collision.transform.position.x < 0)
                    {
                        Destroy(gameObject);
                        return;
                    }
                } else // Derecha
                {
                    if (transform.position.x - collision.transform.position.x > 0)
                    {
                        Destroy(gameObject);
                        return;
                    }
                }
            }
            collision.GetComponent<EnemyController>().HurtEnemy(damage);
            collision.GetComponent<EnemyController>().ActivateInternalDamage();
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
