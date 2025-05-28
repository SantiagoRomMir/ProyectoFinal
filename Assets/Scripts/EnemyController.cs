using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public bool move;
    public bool shield;

    public bool isStatic;

    [Header("Simulation")]
    public bool triggerInternalDamage;
    [Header("HP Bars")]
    public GameObject HPBar;
    public GameObject InternalBar;
    public GameObject BGBar;
    public float hideHPBarDelay;
    [Header("MoneyDrop")]
    public GameObject money;
    public float moneyMultiplier;
    [Header("FootSteps")]
    public AudioSource footStepsAudio;
    private void Awake()
    {
        money.GetComponent<ConsumableController>().consumable = Consumable.TypeConsumable.Money;

        player = GameObject.FindGameObjectWithTag("Player");

        internalDamage = 0;
        lastTimeHurt = Time.time;

        maxHealth = health;

        //BGBar.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
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

        footStepsAudio = GetComponent<AudioSource>();
        footStepsAudio.volume = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().GetSoundSource().volume;
        StartCoroutine("FootStepsSound");
    }
    private void Update()
    {
        GetComponent<Animator>().SetFloat("VelocityX", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));

        if (!isHealingInternalDamage && Time.time > lastTimeHurt + healInternalDamageDelay && internalDamage > 0)
        {
            StartCoroutine("HealInternalDamage");
        }

        if (triggerInternalDamage)
        {
            ActivateInternalDamage();
        }

        else if (move && !isStatic)
        {
            Movement();
            FlipEnemy();
            //GetComponent<Animator>().SetFloat("VelocityX", phisics.velocity.x);
        }

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
            sprite.flipX = true;
        }
        else if (direction == 1)
        {
            sprite.flipX = false;
        }
    }
    public void RandomDrop()
    {
        int probability = Random.Range(0, 4);
        if (probability==2)
        {
            int randomDrop = Random.Range(0, 3);
            Consumable.TypeConsumable typeConsumable;
            switch (randomDrop)
            {
                case 0:
                    typeConsumable = Consumable.TypeConsumable.Datil;
                    break; 
                case 1:
                    typeConsumable = Consumable.TypeConsumable.Pera;
                    break;
                default:
                    typeConsumable = Consumable.TypeConsumable.Hierbabuena;
                    break;
            }
            Instantiate(money, transform.position, Quaternion.identity).GetComponent<ConsumableController>().consumable = typeConsumable;
        }
    }
    private void DropMoney()
    {
        for (int i = 0; i < Random.Range(1, 6) + 1 * moneyMultiplier; i++)
        {
            int randMoney = (int)(Random.Range(5, 16) + 1 * moneyMultiplier);
            money.GetComponent<ConsumableController>().money = randMoney;
            money.transform.position = transform.position;
            Instantiate(money);
        }
    }
    public void HurtEnemy(int damage)
    {
        if (health<=0)
        {
            return;
        }
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().GetSoundSource().PlayOneShot(GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().enemyHurt);
        if (BGBar.GetComponent<Image>().color.a == 0)
        {
            BGBar.GetComponent<Animator>().SetBool("FadeOut", false);
            BGBar.GetComponent<Animator>().SetBool("FadeIn", true);
        }
        StopCoroutine("HealInternalDamage");
        health -= damage;
        //Debug.Log("EnemyHurt: " + damage);
        lastTimeHurt = Time.time;
        isHealingInternalDamage = false;

        if (health <= 0)
        {
            PlayDeathSound();
            GetComponent<Animator>().SetTrigger("Death");
            StopAllCoroutines();
            StartCoroutine("Dead");
        }
    }
    private void PlayDeathSound()
    {
        SoundController soundController = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>();
        AudioClip sound = soundController.enemyDeath[Random.Range(0,soundController.enemyDeath.Length-1)];
        soundController.GetSoundSource().PlayOneShot(sound);
    }
    IEnumerator FootStepsSound()
    {
        while (true)
        {
            if (Mathf.Abs(phisics.velocity.x) > 0f)
            {
                SoundController soundController = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>();
                AudioClip sound = soundController.footSteps[UnityEngine.Random.Range(0, soundController.footSteps.Length - 1)];
                footStepsAudio.PlayOneShot(sound);
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator HealInternalDamage()
    {
        if (BGBar.GetComponent<Image>().color.a == 0)
        {
            BGBar.GetComponent<Animator>().SetBool("FadeOut", false);
            BGBar.GetComponent<Animator>().SetBool("FadeIn", true);
        }
        //Debug.Log("Enemy HealingInternalDamage: " + internalDamage);
        isHealingInternalDamage = true;
        while (internalDamage > 0)
        {
            internalDamage--;
            yield return new WaitForSeconds(0.1f);
        }
        //Debug.Log("Enemy Finished HealingInternalDamage: " + internalDamage);
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
        lastTimeHurt = Time.time;
        isHealingInternalDamage = false;
        if (!player.GetComponent<PlayerController>().hasGun || internalDamage>=health)
        {
            return;
        }
        internalDamage += addInternalDamage;
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
        HPBar.GetComponent<Image>().fillAmount = (float)(health - internalDamage) / maxHealth;
    }
    private void UpdateInternalBar()
    {
        //InternalBar.GetComponent<Image>().fillAmount = (float)(health-internalDamage)/maxHealth;
        InternalBar.GetComponent<Image>().fillAmount = (float)health / maxHealth;
    }
    private void HideHPBar()
    {
        if (Time.time > lastTimeHurt + hideHPBarDelay && !isHealingInternalDamage)
        {
            BGBar.GetComponent<Animator>().SetBool("FadeIn", false);
            BGBar.GetComponent<Animator>().SetBool("FadeOut", true);
        }
    }
    IEnumerator Dead()
    {
        if (GetComponent<MeleeController>()!=null)
        {
            GetComponent<MeleeController>().StopAllCoroutines();
        }
        if (GetComponent<ArcherController>() != null)
        {
            GetComponent<ArcherController>().StopAllCoroutines();
        }
        move  = false;
        health = 0;
        internalDamage = 0;
        phisics.velocity = new Vector2(0, phisics.velocity.y);
        yield return new WaitForSeconds(1.25f);
        RandomDrop();
        DropMoney();
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }
}