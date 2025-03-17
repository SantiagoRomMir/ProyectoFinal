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

    [Header("Shoot")]
    public GameObject bulletPrefab;
    public float reloadTime;
    private bool canShoot;
    private bool isReloading;

    [Header("Dodge")]
    public float dodgeDuration;
    public float dodgeSpeed;
    public float dodgeCooldown;
    private float lastTimeDodge;
    private bool canMove;

    [Header("Controls")]
    public KeyCode attackKey;
    public KeyCode parryKey;
    public KeyCode shootKey;
    public KeyCode reloadKey;
    public KeyCode dodgeKey;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackCounter = 0;
        lastTimeAttack = Time.time;
        lastFinishedCombo = Time.time;

        isVulnerable = true;
        lastTimeParry = Time.time;
        lastTimeHurt = Time.time;
        internalDamage = 0;
        canShoot = true;

        lastTimeDodge = Time.time;
        canMove = true;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            direction = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
            if (direction < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (direction > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            direction = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
            if (direction < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (direction > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }
        // Update is called once per frame
    void Update() 
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, radio, suelo);

        if (Input.GetKeyDown(attackKey))
        {
            Attack();
        }
        CheckAttackCombo();

        if (Input.GetKeyDown(parryKey) && Time.time>=lastTimeParry+parryCooldown)
        {
            StartCoroutine("Parry");
        }
        if ((Input.GetKeyUp(parryKey) || Time.time>=lastTimeParry+parryDuration) && parry.activeSelf)
        {
            StopCoroutine("Parry");
            parry.SetActive(false);
            isVulnerable = true;
            lastTimeParry = Time.time;
        }

        if (Input.GetKeyDown(shootKey) && canShoot)
        {
            Shoot();
        }

        if (Input.GetKeyDown(reloadKey) && !canShoot && !isReloading)
        {
            StartCoroutine("Reload");
        }

        if (!isHealingInternalDamage && Time.time > lastTimeHurt + healInternalDamageDelay && internalDamage > 0)
        {
            StartCoroutine("HealInternalDamage");
        }

        if (Input.GetKeyDown(dodgeKey) && Time.time > lastTimeDodge + dodgeCooldown)
        {
            StartCoroutine("Dodge");
        }

        if (extraJumps == false && isGrounded == true)
        {
            extraJumps = true;
        }
        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpforce;
        }
        else if (isGrounded == false && Input.GetKeyDown(KeyCode.Space) && extraJumps == true)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpforce;
            extraJumps = false;
        }
        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpforce;
                jumpTimeCounter -= Time.deltaTime;

            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
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
        weapon.GetComponent<FlipWeapon>().FlipPosition();
        weapon.SetActive(true);
        weapon.GetComponent<Collider2D>().enabled = false;
        weapon.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        weapon.SetActive(false);
    }
    private void Shoot()
    {
        canShoot = false;
        float forwardDir = GetFacingDirection();
        bulletPrefab.GetComponent<BulletController>().direction = forwardDir;
        Instantiate(bulletPrefab, new Vector2(transform.position.x + forwardDir, transform.position.y), Quaternion.identity);
    }
    private int GetFacingDirection()
    {
        if (GetComponent<SpriteRenderer>().flipX)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
        isReloading = false;
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
        isVulnerable = false;
        parry.GetComponent<ParryController>().FlipPosition();
        parry.SetActive(true);
        parry.GetComponent<Collider2D>().enabled = false;
        parry.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(perfectParryTimeWindow);
        parry.GetComponent<SpriteRenderer>().color = Color.red;
        parry.GetComponent<ParryController>().isPerfect = false;
        yield return new WaitForSeconds(parryDuration);
        lastTimeParry = Time.time;
        parry.SetActive(false);
        isVulnerable = true;
    }
    IEnumerator Dodge()
    {
        canMove = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        lastTimeDodge = Time.time;
        isVulnerable = false;
        int dir = GetFacingDirection();
        Debug.Log(GetFacingDirection());
        rb.velocity = Vector3.zero;
        do
        {
            rb.velocity += Vector2.right*(dodgeSpeed * dir);
            yield return new WaitForEndOfFrame();
        } while (Time.time - lastTimeDodge <= dodgeDuration);
        isVulnerable = true;
        rb.constraints = RigidbodyConstraints2D.None;
        canMove = true;
        lastTimeDodge = Time.time;
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