using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    public enum Dificultad
    {
        facil,
        normal,
        dificil
    }
    public Dificultad dificultad;
    public float speedEenemy;
    private Vector3 posInitial;
    private int stop = 1;
    public Transform posEnd;
    private float direction;
    private bool movetoEnd = true;
    private SpriteRenderer sprite;
    private Rigidbody2D phisics;
    private Animator anim;
    public GameObject[] drop;
    public int[] posbilidades;
    private int numeroDrop;
    public bool move;
    public bool chase;

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
        /*if((int)dificultad>PlayerPrefs.GetInt("dificultad")){
            Destroy(gameObject);
        }*/
        posInitial = transform.position;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        if (posEnd.localPosition.x > transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
        phisics = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
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
        if (chase)
        {
            Chase();
        }
        else if (move)
        {
            Movement();
            FlipEnemy();
        }
        else
        {
            FlipStoppedEnemy();
        }
    }
    private void Chase()
    {
        if (transform.position.x - player.transform.position.x < 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
        phisics.velocity = new Vector2(direction * speedEenemy /** PlayerPrefs.GetFloat("dificultadV")*/ * stop, phisics.velocity.y);

    }
    private void Movement()
    {
        phisics.velocity = new Vector2(direction * speedEenemy /** PlayerPrefs.GetFloat("dificultadV")*/ * stop, phisics.velocity.y);

        if (posEnd.localPosition.x < posInitial.x)
        {
            if (transform.localPosition.x < posEnd.localPosition.x && movetoEnd == true)
            {
                direction *= -1;
                movetoEnd = false;
            }
            Debug.Log(posInitial.x);
            if (transform.position.x > posInitial.x && movetoEnd == false)
            {
                direction *= -1;
                movetoEnd = true;
            }
        }
        else
        {
            if (transform.position.x > posEnd.localPosition.x && movetoEnd == true)
            {
                direction *= -1;
                movetoEnd = false;
            }
            if (transform.position.x < posInitial.x && movetoEnd == false)
            {
                direction *= -1;
                movetoEnd = true;
            }
        }
    }
    private void FlipStoppedEnemy()
    {
        if (transform.position.x < player.transform.position.x)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }
    private void FlipEnemy()
    {
        if (phisics.velocity.x < -0f)
        {
            sprite.flipX = false;
        }
        else if (phisics.velocity.x > 0f)
        {
            sprite.flipX = true;
        }
    }
    public void RandomDrop()
    {
        numeroDrop = Random.Range(0, 100);
        for (int i = 0; i < drop.Length; i++)
        {
            if (posbilidades[i] < numeroDrop)
            {
                Instantiate(drop[i], transform.position, Quaternion.identity);
            }
        }
    }
    public void HurtEnemy(int damage)
    {
        StopCoroutine("HealInternalDamage");
        health -= damage;
        Debug.Log("EnemyHurt: " + damage);
        lastTimeHurt = Time.time;
        isHealingInternalDamage = false;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator Attack()
    {
        while (true)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
            {
                StartCoroutine("AttackAnim");
            }
            yield return new WaitForSeconds(attackCooldown);
        }
    }
    IEnumerator HealInternalDamage()
    {
        Debug.Log("Enemy HealingInternalDamage: " + internalDamage);
        isHealingInternalDamage = true;
        while (internalDamage > 0)
        {
            internalDamage--;
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Enemy Finished HealingInternalDamage: " + internalDamage);
        isHealingInternalDamage = false;
    }
    IEnumerator AttackAnim()
    {
        stop = 0;
        weapon.SetActive(true);
        weapon.GetComponent<Collider2D>().enabled = false;
        weapon.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        stop = 1;
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
        if (internalDamage <= 0)
        {
            return;
        }
        HurtEnemy(internalDamage);
        internalDamage = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            chase = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            chase = false;
            if (Vector2.Distance(posEnd.position, transform.position) < Vector2.Distance(posInitial, transform.position))
            {
                movetoEnd = false;
                if (transform.position.x < posInitial.x)
                {
                    direction = 1;
                }
                else
                {
                    direction = 1;
                }
            }
            else
            {
                movetoEnd = true;
                if (transform.position.x < posEnd.localPosition.x)
                {
                    direction = 1;
                }
                else
                {
                    direction = 1;
                }
            }
        }


    }
}
