using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public int damage;
    public string hitTag;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(transform.parent.tag+" AttackHit: "+collision.tag);
        if (collision != null)
        {
            if (collision.CompareTag(hitTag))
            {
                switch (hitTag)
                {
                    case "Player":
                        collision.GetComponent<PlayerController>().HurtPlayer(damage, transform.position, false, true);
                        break;
                    case "Enemy":
                        collision.GetComponent<EnemyController>().HurtEnemy(damage);
                        break;
                }
            } else if (hitTag.Equals("Enemy") && collision.CompareTag("Vase"))
            {
                Debug.Log("VaseHit");
                collision.GetComponent<VaseController>().DropMoney();
            }
        }
    }
}
