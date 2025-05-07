using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaCañon : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
}
