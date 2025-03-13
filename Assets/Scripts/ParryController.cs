using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryController : MonoBehaviour
{
    public int internalDamage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("EnemyAttack"))
        {
            collision.transform.parent.GetComponent<EnemyController>().InternalHurtEnemy(internalDamage);
        }
    }
}
