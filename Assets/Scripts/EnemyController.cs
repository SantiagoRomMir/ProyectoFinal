using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public int health;
    private int maxHealth;
    public int internalDamage;
    public GameObject player;
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
    public Vector3 posInitial;
    public int stop = 1;
    public Transform posEnd;
    public float direction;
    public bool movetoEnd = true;
    private SpriteRenderer sprite;
    public Rigidbody2D phisics;
    private Animator anim;
    public GameObject[] drop;
    public int[] posbilidades;
    private int numeroDrop;
    public bool move;

    [Header("Simulation")]
    public bool triggerInternalDamage;
    [Header("HP Bars")]
    public GameObject HPBar;
    public GameObject InternalBar;
    public GameObject BGBar;
    public float hideHPBarDelay;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        internalDamage = 0;
        lastTimeHurt = Time.time;

        maxHealth = health;

        BGBar.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
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
        
        else if (move)
        {
            Movement();

        }
        FlipEnemy();

        UpdateHPBar();
        UpdateInternalBar();

        HideHPBar();
    }
    
    private void Movement()
    {
        phisics.velocity = new Vector2(direction * speedEenemy /* PlayerPrefs.GetFloat("dificultadV")*/ * stop, phisics.velocity.y);

        if (posEnd.localPosition.x < posInitial.x)
        {
            if (transform.localPosition.x < posEnd.localPosition.x && movetoEnd == true)
            {
                direction *= -1;
                movetoEnd = false;
            }
            if (transform.position.x > posInitial.x && movetoEnd == false)
            {
                direction *= -1;
                movetoEnd = true;
            }
        }
        else
        {
            if (transform.position.x > posEnd.position.x && movetoEnd == true)
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
        if (direction == -1)
        {
            sprite.flipX = false;
        }
        else if (direction == 1)
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
        if (BGBar.GetComponent<Image>().color.a == 0)
        {
            BGBar.GetComponent<Animator>().SetBool("FadeOut", false);
            BGBar.GetComponent<Animator>().SetBool("FadeIn",true);
        }
        StopCoroutine("HealInternalDamage");
        health -= damage;
        Debug.Log("EnemyHurt: " + damage);
        lastTimeHurt = Time.time;
        isHealingInternalDamage = false;

        if (health <= 0)
        {
            Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }
    
    IEnumerator HealInternalDamage()
    {
        if (BGBar.GetComponent<Image>().color.a == 0)
        {
            BGBar.GetComponent<Animator>().SetBool("FadeOut", false);
            BGBar.GetComponent<Animator>().SetBool("FadeIn", true);
        }
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
    
    public void InternalHurtEnemy(int addInternalDamage)
    {
        if (BGBar.GetComponent<Image>().color.a == 0)
        {
            BGBar.GetComponent<Animator>().SetBool("FadeOut", false);
            BGBar.GetComponent<Animator>().SetBool("FadeIn", true);
        }
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
    private void UpdateHPBar()
    {
        //HPBar.GetComponent<Image>().fillAmount = (float)health/maxHealth;
        HPBar.GetComponent<Image>().fillAmount = (float)(health-internalDamage)/maxHealth;
    }
    private void UpdateInternalBar()
    {
        //InternalBar.GetComponent<Image>().fillAmount = (float)(health-internalDamage)/maxHealth;
        InternalBar.GetComponent<Image>().fillAmount = (float)health/maxHealth;
    }
    private void HideHPBar()
    {
        if (Time.time>lastTimeHurt+hideHPBarDelay && !isHealingInternalDamage)
        {
            BGBar.GetComponent<Animator>().SetBool("FadeIn", false);
            BGBar.GetComponent<Animator>().SetBool("FadeOut", true);
        }
    }
}
