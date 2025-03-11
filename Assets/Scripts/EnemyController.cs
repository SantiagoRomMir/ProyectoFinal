using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float lastTimeHurt;
    private void Awake()
    {
        lastTimeHurt = Time.time;
    }
    public void HurtEnemy(int damage)
    {
        if (Time.time < lastTimeHurt + 0.5f)
        {
            Debug.Log("CantHurt");
            return;
        }
        Debug.Log("Damage: " + damage);
        lastTimeHurt = Time.time;
    }
}
