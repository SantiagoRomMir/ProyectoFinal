using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private float direction;
    private bool isGrounded;
    public Transform feetPos;
    public float radio;
    public LayerMask suelo;
    public float jumpforce;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool isJumping;
    private bool extraJumps;

    [Header("MeleeAttack")]
    public GameObject weapon;
    public int damage;
    public int attackNumberChain;
    public float attackTimeChainReset;
    private float lastTimeAttack;
    public float attackCooldown;
    private float lastFinishedCombo;
    public float finishComboCooldown;
    private int attackCounter;

    [Header("Parry")]
    public GameObject parry;
    private bool isVulnerable;
    public float parryCooldown;
    public float parryDuration;
    private float lastTimeParry;
    public float perfectParryTimeWindow;
    public int internalDamage;
    private float lastTimeHurt;
    private bool isHealingInternalDamage;
    public float healInternalDamageDelay;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        attackCounter = 0;
        lastTimeAttack = Time.time;
        lastFinishedCombo = Time.time;

        isVulnerable = true;
        lastTimeParry = Time.time;
        lastTimeHurt = Time.time;
        internalDamage = 0;
    }

    void FixedUpdate()
    {
        direction=Input.GetAxis("Horizontal");
        rb.velocity=new Vector2(direction*speed,rb.velocity.y);
        if(direction<0){
            GetComponent<SpriteRenderer>().flipX=true;
        }else if(direction>0){
            GetComponent<SpriteRenderer>().flipX=false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
        }
        CheckAttackCombo();

        if (Input.GetKeyDown(KeyCode.F) && Time.time>=lastTimeParry+parryCooldown)
        {
            StartCoroutine("Parry");
        }
        if (Input.GetKeyUp(KeyCode.F) || Time.time>=lastTimeParry+parryDuration)
        {
            StopCoroutine("Parry");
            parry.SetActive(false);
            isVulnerable = true;
        }

        if (!isHealingInternalDamage && Time.time > lastTimeHurt + healInternalDamageDelay && internalDamage > 0)
        {
            StartCoroutine("HealInternalDamage");
        }

        isGrounded =Physics2D.OverlapCircle(feetPos.position,radio,suelo);
        if(extraJumps==false && isGrounded==true){
            extraJumps=true;
        }
        if(isGrounded==true && Input.GetKeyDown(KeyCode.Space)){
            isJumping=true;
            jumpTimeCounter=jumpTime;
            rb.velocity=Vector2.up*jumpforce;
        }else if(isGrounded==false && Input.GetKeyDown(KeyCode.Space) && extraJumps==true){
            isJumping=true;
            jumpTimeCounter=jumpTime;
            rb.velocity=Vector2.up*jumpforce;
            extraJumps=false;
        }
        if(Input.GetKey(KeyCode.Space) && isJumping==true){
            if(jumpTimeCounter>0){
                rb.velocity=Vector2.up*jumpforce;
                jumpTimeCounter-=Time.deltaTime;

            }else{
                isJumping=false;
            }
        }
        if(Input.GetKeyUp(KeyCode.Space)){
            isJumping=false;
        }
    }
    public void Attack()
    {
        if (Time.time < lastTimeAttack+attackCooldown || Time.time < lastFinishedCombo + finishComboCooldown)
        {
            return;
        }
        lastTimeAttack = Time.time;
        attackCounter++;
        if (attackCounter == 1)
        {
            weapon.GetComponent<WeaponController>().damage = damage;
            weapon.GetComponent<SpriteRenderer>().color = Color.yellow;
            
        }
        else if (attackCounter == 2)
        {
            weapon.GetComponent<WeaponController>().damage += damage*50/100;
            weapon.GetComponent<SpriteRenderer>().color = new Color32(250,156,28,255);
        }
        else if (attackCounter == 3)
        {
            weapon.GetComponent<WeaponController>().damage += damage * 100 / 100;
            weapon.GetComponent<SpriteRenderer>().color = Color.red;
            attackCounter = 0;
            lastFinishedCombo = Time.time;
        }
        StartCoroutine("AttackAnim");
    }
    IEnumerator AttackAnim()
    {
        weapon.SetActive(true);
        weapon.GetComponent<Collider2D>().enabled = false;
        weapon.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        weapon.SetActive(false);
    }
    private void CheckAttackCombo()
    {
        if (Time.time > lastTimeAttack + attackTimeChainReset && attackCounter > 0)
        {
            Debug.Log("ResetCombo");
            attackCounter = 0;
        }
    }
    public void HurtPlayer(int damage)
    {
        if (!isVulnerable)
        {
            return;
        }
        Debug.Log("PlayerHurt: " + (damage + internalDamage));
        internalDamage = 0;
        lastTimeHurt = Time.time;
    }
    IEnumerator Parry()
    {
        parry.GetComponent<ParryController>().isPerfect = true;
        parry.GetComponent<SpriteRenderer>().color = Color.green;
        lastTimeParry = Time.time;
        isVulnerable = false;
        parry.SetActive(true);
        parry.GetComponent<Collider2D>().enabled = false;
        parry.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(perfectParryTimeWindow);
        parry.GetComponent<SpriteRenderer>().color = Color.red;
        parry.GetComponent<ParryController>().isPerfect = false;
        yield return new WaitForSeconds(parryDuration);
        parry.SetActive(false);
        isVulnerable = true;
    }
    public void InternalHurtPlayer(int addInternalDamage)
    {
        StopCoroutine("HealInternalDamage");
        internalDamage += addInternalDamage;
        lastTimeHurt = Time.time;
        isHealingInternalDamage = false;
    }
    IEnumerator HealInternalDamage()
    {
        Debug.Log("Player HealingInternalDamage: " + internalDamage);
        isHealingInternalDamage = true;
        while (internalDamage > 0)
        {
            internalDamage--;
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Player Finished HealingInternalDamage: " + internalDamage);
        isHealingInternalDamage = false;
    }
}
