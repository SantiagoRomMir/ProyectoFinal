using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MeleeController : MonoBehaviour
{
    public bool chase;
    public GameObject weapon;
    public GameObject shield;
    public int damage;
    public float attackCooldown;
    public float attackRange;

    private void Awake()
    {
        chase = false;
        weapon.GetComponent<WeaponController>().damage = damage;
    }

    private void Start()
    {
        StartCoroutine("Attack");
    }
    private void Update()
    {
        if (chase)
        {
            Chase();
        }
        FlipWeapon();
    }
    private void FlipWeapon()
    {
        if (GetComponent<EnemyController>().direction == -1)
        {
            weapon.transform.localPosition = new Vector2(Mathf.Abs(weapon.transform.localPosition.x) * -1, weapon.transform.localPosition.y);
            if (shield!=null)
            {
                shield.transform.localPosition = new Vector2(Mathf.Abs(shield.transform.localPosition.x) * -1, shield.transform.localPosition.y);
            }
        }
        else if (GetComponent<EnemyController>().direction == 1)
        {
            weapon.transform.localPosition = new Vector2(Mathf.Abs(weapon.transform.localPosition.x), weapon.transform.localPosition.y);
            if (shield != null)
            {
                shield.transform.localPosition = new Vector2(Mathf.Abs(shield.transform.localPosition.x), shield.transform.localPosition.y);
            }
        }
    }
    private void Chase()
    {
        if (Vector2.Distance(transform.position, GetComponent<EnemyController>().player.transform.position) < 1.5f)
        {
            GetComponent<EnemyController>().stop = 0;
            if (transform.position.x - GetComponent<EnemyController>().player.transform.position.x < 0)
            {
                GetComponent<EnemyController>().direction = 1;
            }
            else
            {
                GetComponent<EnemyController>().direction = -1;
            }
        }
        else
        {
            GetComponent<EnemyController>().stop = 1;
        }
        if (transform.position.x - GetComponent<EnemyController>().player.transform.position.x < 0)
        {
            GetComponent<EnemyController>().direction = 1;
        }
        else
        {
            GetComponent<EnemyController>().direction = -1;
        }
        GetComponent<EnemyController>().phisics.velocity = new Vector2(GetComponent<EnemyController>().direction * GetComponent<EnemyController>().speedEenemy /* PlayerPrefs.GetFloat("dificultadV")*/ * GetComponent<EnemyController>().stop, GetComponent<EnemyController>().phisics.velocity.y);


    }
    IEnumerator AttackAnim()
    {
        GetComponent<EnemyController>().stop = 0;
        weapon.GetComponent<Collider2D>().enabled = true;
        weapon.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GetComponent<EnemyController>().stop = 1;
        weapon.GetComponent<Collider2D>().enabled = false;
        weapon.SetActive(false);
    }
    IEnumerator Attack()
    {
        while (true)
        {
            if (Vector2.Distance(transform.position, GetComponent<EnemyController>().player.transform.position) <= attackRange)
            {
                StartCoroutine("AttackAnim");
            }
            yield return new WaitForSeconds(attackCooldown);
        }
    }
    public void ReturnPatrol()
    {
        if (Vector2.Distance(GetComponent<EnemyController>().posEnd.position, transform.position) < Vector2.Distance(GetComponent<EnemyController>().posInitial, transform.position))
        {
            GetComponent<EnemyController>().movetoEnd = false;
            if (transform.position.x < GetComponent<EnemyController>().posInitial.x)
            {
                GetComponent<EnemyController>().direction = 1;
            }
            else
            {
                GetComponent<EnemyController>().direction = -1;
            }
        }
        else
        {
            GetComponent<EnemyController>().movetoEnd = true;
            if (transform.position.x < GetComponent<EnemyController>().posEnd.localPosition.x)
            {
                GetComponent<EnemyController>().direction = 1;
            }
            else
            {
                GetComponent<EnemyController>().direction = -1;
            }
        }
    }
}
