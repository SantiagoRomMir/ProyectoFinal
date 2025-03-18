using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health;
    public int internalDamage;
    public GameObject weapon;
    public int damage;
    public float attackCooldown;
    public float attackRange;
    private GameObject player;
    private float lastTimeHurt;
    private bool isHealingInternalDamage;
    public float healInternalDamageDelay;

    [Header("Simulation")]
    public bool triggerInternalDamage;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weapon.GetComponent<WeaponController>().damage = damage;

        internalDamage = 0;
        lastTimeHurt = Time.time;
    }
    private void Start()
    {
        StartCoroutine("Attack");
    }
    private void Update()
    {
        if (!isHealingInternalDamage && Time.time > lastTimeHurt + healInternalDamageDelay && internalDamage > 0)
        {
            StartCoroutine("HealInternalDamage");
        }

        if (triggerInternalDamage)
        {
            ActivateInternalDamage();
        }
    }
    public void HurtEnemy(int damage)
    {
        StopCoroutine("HealInternalDamage");
        health -= damage;
        Debug.Log("EnemyHurt: " + damage);
        lastTimeHurt = Time.time;
        isHealingInternalDamage = false;

        if (health<=0)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator Attack()
    {
        while (true)
        {
            if (Vector2.Distance(transform.position, player.transform.position)<=attackRange)
            {
                StartCoroutine("AttackAnim");
            }
            yield return new WaitForSeconds(attackCooldown);
        }
    }
    IEnumerator HealInternalDamage()
    {
        Debug.Log("Enemy HealingInternalDamage: "+internalDamage);
        isHealingInternalDamage = true;
        while (internalDamage>0) {
            internalDamage--;
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Enemy Finished HealingInternalDamage: " + internalDamage);
        isHealingInternalDamage = false;
    }
    IEnumerator AttackAnim()
    {
        weapon.SetActive(true);
        weapon.GetComponent<Collider2D>().enabled = false;
        weapon.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        weapon.SetActive(false);
    }
    public void InternalHurtEnemy(int addInternalDamage)
    {
        StopCoroutine("HealInternalDamage");
        internalDamage += addInternalDamage;
        lastTimeHurt = Time.time;
        isHealingInternalDamage = false;
    }
    public void ActivateInternalDamage()
    {
        if (internalDamage<=0)
        {
            return;
        }
        HurtEnemy(internalDamage);
        internalDamage = 0;
    }
}
