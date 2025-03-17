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
        if (collision != null && collision.CompareTag(hitTag))
        {
            switch (hitTag)
            {
                case "Player":
                    collision.GetComponent<PlayerController>().HurtPlayer(damage);
                    break;
                case "Enemy":
                    collision.GetComponent<EnemyController>().HurtEnemy(damage);
                    break;
            }
        }
    }
}
