using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryController : MonoBehaviour
{
    public int internalDamage;
    public bool isPerfect;
    public int internalDamagePlayerPercent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("EnemyAttack"))
        {
            if (isPerfect)
            {
                collision.transform.parent.GetComponent<EnemyController>().InternalHurtEnemy(internalDamage);
                Debug.Log("Perfect Parry");
            } else
            {
                transform.parent.GetComponent<PlayerController>().InternalHurtPlayer(internalDamage*internalDamagePlayerPercent/100);
                Debug.Log("Partial Parry");
            }
        }
    }
}
