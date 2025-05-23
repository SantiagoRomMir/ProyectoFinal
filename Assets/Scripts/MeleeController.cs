using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MeleeController : MonoBehaviour
{
    public bool chase;
    public GameObject weapon;
    public int damage;
    public float attackCooldown;
    public float attackRange;
    public float attackDelay;

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
        transform.parent.GetChild(2).transform.position = new Vector2(transform.parent.GetChild(2).transform.position.x, transform.position.y);
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
        }
        else if (GetComponent<EnemyController>().direction == 1)
        {
            weapon.transform.localPosition = new Vector2(Mathf.Abs(weapon.transform.localPosition.x), weapon.transform.localPosition.y);
        }
    }
    private void Chase()
    {
        if (!GetComponent<EnemyController>().move)
        {
            return;
        }
        if (Vector2.Distance(transform.position, GetComponent<EnemyController>().player.transform.position) < 1.25f)
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
        GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(attackDelay);
        weapon.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GetComponent<EnemyController>().stop = 1;
        weapon.SetActive(false);
    }
    IEnumerator Attack()
    {
        while (true)
        {
            if (Vector2.Distance(transform.position, GetComponent<EnemyController>().player.transform.position) <= attackRange)
            {
                GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().GetSoundSource().PlayOneShot(GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().enemyAttack);
                StartCoroutine("AttackAnim");
                yield return new WaitForSeconds(attackCooldown);
            }
            yield return new WaitForEndOfFrame();
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
