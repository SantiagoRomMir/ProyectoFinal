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
    private float attackCooldown;
    private float lastFinishedCombo;
    private float finishComboCooldown;
    public int attackCounter;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        attackCounter = 0;
        attackTimeChainReset = 1f;
        attackNumberChain = 3;
        lastTimeAttack = Time.time;
        attackCooldown = 0.2f;
        lastFinishedCombo = Time.time;
        finishComboCooldown = 0.5f;
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

        isGrounded=Physics2D.OverlapCircle(feetPos.position,radio,suelo);
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
            weapon.GetComponent<WeaponController>().damage += damage*25/100;
            weapon.GetComponent<SpriteRenderer>().color = new Color32(250,156,28,255);
        }
        else if (attackCounter == 3)
        {
            weapon.GetComponent<WeaponController>().damage += damage * 50 / 100;
            weapon.GetComponent<SpriteRenderer>().color = Color.red;
            attackCounter = 0;
            lastFinishedCombo = Time.time;
        }
        StopCoroutine("AttackAnim");
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
}
