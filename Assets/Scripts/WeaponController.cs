using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public int damage;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>().HurtEnemy(damage);
        }
    }
}
