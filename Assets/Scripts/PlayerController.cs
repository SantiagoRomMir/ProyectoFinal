using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject aim;
    public GameObject loro;
    private Rigidbody2D rb;
    private LineRenderer line;
    public Canvas canvas;
    private HudControl hudControl;
    public List<Transform> ganchos;
    public float speed;
    private float direction;
    private float verticalDirection;
    private bool isHooking;

    public Transform feetPos;
    public Transform firePosition;
    private Transform ganchoCercano;
    public Vector3 lastPosition;

    public float radio;
    public LayerMask suelo;
    public LayerMask player;
    public float jumpforce;
    public float jumpTime;
    private float jumpTimeCounter;

    public int maxHp;
    private int hp;
    public int maxRon;
    private int ron;

    private bool isGrounded;
    private bool isJumping;
    private bool isCrouching;
    private bool extraJumps;
    private bool isFalling;
    private bool aiming;
    public bool usingLoro;
    public float Slowed = 1;

    public float invulnerableTime;

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
    private bool isLookingUp;

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
    public bool canShoot;
    public bool isReloading;

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

    [Header("Sound")]
    private AudioSource audioSource;

    [Header("Skills")]
    public bool hasHook;
    public bool hasParrot;
    public bool hasGun;

    [Header("Consumables")]
    public int selectedConsumable;
    public float defense;
    public float consumableCooldown;
    public int addedDamage;
    public int money;
    public List<Consumable> consumables;
    private float lastConsumableTime;

    [Header("Persistence")]
    public bool clearPersistenceData; // If True Ignore Persistent Data and Overwrite it
    private Persistence persistence;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("clearPersistenceData")==1)
        {
            PlayerPrefs.SetInt("clearPersistenceData", 0);
            clearPersistenceData = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Hud").GetComponent<Canvas>();
        hudControl=canvas.GetComponent<HudControl>();
        consumables = new List<Consumable>();
        selectedConsumable = 0;
        defense = 1f;
        lastConsumableTime = Time.time;

        audioSource = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().GetSoundSource();
        if (PlayerPrefs.GetString("accion") == "puerta")
        {
            if (GameObject.Find(PlayerPrefs.GetString("Door"))!=null)
            {
                transform.position = GameObject.Find(PlayerPrefs.GetString("Door")).transform.position;
            }
        }
        if (PlayerPrefs.GetString("accion") == "Respawning")
        {
            if (GameObject.Find(PlayerPrefs.GetString("positionRespawn"))!=null)
            {
                transform.position = GameObject.Find(PlayerPrefs.GetString("positionRespawn")).transform.position;
            }
        }
        rb = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        GameObject door = GameObject.Find(PlayerPrefs.GetString("Door"));
        if (door != null)
        {
            transform.position = door.transform.position;
        }
        rb = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        ron = maxRon;
        hp = maxHp;
        hp = 10;
        isHooking = false;
        usingLoro = false;
        attackCounter = 0;
        lastTimeAttack = Time.time;
        lastFinishedCombo = Time.time;
        hp = maxHp;
        isVulnerable = true;
        lastTimeParry = Time.time;
        lastTimeHurt = Time.time;
        internalDamage = 0;
        canShoot = true;

        lastTimeDodge = Time.time;
        canMove = true;

        hasHook = false;
        hasGun = false;
        hasParrot = false;

        LoadPersistenceData();

        StartCoroutine("AttackUpwards");
    }

    void FixedUpdate()
    {
        if (!usingLoro && canMove)
        {
            if (!aiming)
            {
                direction = Input.GetAxis("Horizontal");
                rb.velocity = new Vector2(direction * speed * Slowed, rb.velocity.y);
                if (direction < 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    if (!isLookingUp)
                    {
                        weapon.transform.localPosition = new Vector2(Mathf.Abs(weapon.transform.localPosition.x) * -1, weapon.transform.localPosition.y);
                    }
                    parry.transform.localPosition = new Vector2(Mathf.Abs(parry.transform.localPosition.x) * -1, parry.transform.localPosition.y);
                }
                else if (direction > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                    if (!isLookingUp)
                    {
                        weapon.transform.localPosition = new Vector2(Mathf.Abs(weapon.transform.localPosition.x), weapon.transform.localPosition.y);
                    }
                    parry.transform.localPosition = new Vector2(Mathf.Abs(parry.transform.localPosition.x), parry.transform.localPosition.y);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(!usingLoro){
            if (hasHook)
            {
                Aim();
            }
            Grounded();
            if (isGrounded)
            {
                Crouching();
            }

            if (isCrouching)
            {
                Traspass();
            }
            else
            {
                if (!isHooking)
                {
                    Jump();
                }
            }

            if (Input.GetKeyDown(parryKey) && Time.time >= lastTimeParry + parryCooldown)
            {
                StartCoroutine("Parry");
            }

            if (Input.GetKeyUp(parryKey) || (Time.time >= lastTimeParry + parryDuration))
            {
                if (parry.activeSelf)
                {
                    StopCoroutine("Parry");
                    parry.SetActive(false);
                    isVulnerable = true;
                    lastTimeParry = Time.time;
                    canMove = true;
                }
            }
            UseConsumable();
        }

        if (!isHealingInternalDamage && Time.time > lastTimeHurt + healInternalDamageDelay && internalDamage > 0)
        {
            StartCoroutine("HealInternalDamage");
        }

        CheckAttackCombo();
    }
    IEnumerator AttackUpwards()
    {
        float distanceFromPlayer = weapon.transform.localPosition.x;
        Debug.Log(distanceFromPlayer);
        while (true)
        {
            Debug.Log(isLookingUp);
            if (Input.GetKey(KeyCode.UpArrow))
            {
                isLookingUp = true;
                weapon.transform.localPosition = new Vector2(0, 1);
            }
            else
            {
                isLookingUp = false;
            }
            if (weapon.transform.localPosition.y>0 && !isLookingUp)
            {
                weapon.transform.localPosition = new Vector2(distanceFromPlayer, 0);
            }
            yield return new WaitForEndOfFrame();
        }
        
    }
    private void LoadPersistenceData()
    {
        if (clearPersistenceData)
        {
            persistence = null;
            Debug.Log("Skipping Persistence Load");
            return;
        }

        persistence = Persistence.LoadPersistence();

        if (persistence != null)
        {
            hp = persistence.hp;
            ron = persistence.ron;
            internalDamage = persistence.internalDamage;
            selectedConsumable = persistence.selectedConsumable;
            addedDamage = persistence.addedDamage;
            defense = persistence.defense;
            hasHook = persistence.hasHook;
            hasParrot = persistence.hasParrot;
            hasGun = persistence.hasGun;
            canShoot = persistence.canShoot;
        } else
        {
            SavePersistenceData();
        }
    }
    public void SavePersistenceData()
    {
        persistence = new Persistence(hp, ron, internalDamage, selectedConsumable, addedDamage, defense, hasHook, hasParrot, hasGun, canShoot);

        persistence.SavePersistence();
    }
    public void AddMoney(int money)
    {
        this.money += money;
    }
    public void AddConsumable(ConsumableController consumable)
    {
        foreach (Consumable c in consumables)
        {
            if (c.consumable.ToString().ToLower().Equals(consumable.consumable.ToString().ToLower())) // Stack Repeated Consumables
            {
                c.remainingAmount++;
                return;
            }
        }
        Consumable newConsumable = new Consumable(consumable);
        consumables.Add(newConsumable);
    }
    public void AddConsumable(Consumable consumable)
    {
        foreach (Consumable c in consumables)
        {
            if (c.consumable.ToString().ToLower().Equals(consumable.consumable.ToString().ToLower())) // Stack Repeated Consumables
            {
                c.remainingAmount++;
                return;
            }
        }
        consumables.Add(new Consumable(consumable));
    }
    public void RemoveConsumable(Consumable consumable)
    {
        consumables.Remove(consumable);
    }
    public void UseConsumable()
    {
        if (consumables.Count<=0)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.G) && Time.time > lastConsumableTime+consumableCooldown)
        {
            lastConsumableTime = Time.time;
            consumables[selectedConsumable].OnUseAction();
        }
    }
    public void SelectNextConsumable()
    {
        selectedConsumable++;
        if (selectedConsumable > consumables.Count - 1)
        {
            selectedConsumable = 0;
        }
    }
    public void SelectPreviousConsumable()
    {
        selectedConsumable--;
        if (selectedConsumable < 0)
        {
            selectedConsumable = consumables.Count - 1;
        }
    }
    public void StartHpRegen(float duration)
    {
        StopCoroutine("RegenHealth");
        StartCoroutine(RegenHealth(duration));
    }
    IEnumerator RegenHealth(float duration)
    {
        float regenStart = Time.time;
        while (Time.time <= regenStart + duration)
        {
            HealPlayer(5);
            yield return new WaitForSeconds(1f);
        }
    }
    public void StartDefenseBuff(float duration)
    {
        StopCoroutine("DefenseBuff");
        StartCoroutine(DefenseBuff(duration));
    }
    IEnumerator DefenseBuff(float duration)
    {
        defense = 1.5f;
        yield return new WaitForSeconds(duration);
        defense = 1f;
    }
    public void StartDamageBuff(float duration)
    {
        StopCoroutine("DamageBuff");
        StartCoroutine(DamageBuff(duration));
    }
    IEnumerator DamageBuff(float duration)
    {
        addedDamage = 2;
        yield return new WaitForSeconds(duration);
        addedDamage = 0;
    }
    public void StartReload()
    {
        if (canShoot || isReloading || !hasGun)
        {
            return;
        }
        StartCoroutine("Reload");
    }
    public void StartDodge()
    {
        if (Time.time <= lastTimeDodge + dodgeCooldown || isHooking)
        {
            return;
        }
        StartCoroutine("Dodge");
    }
    private void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpforce;
        }
        else if (isGrounded == false && Input.GetKeyDown(KeyCode.Space) && extraJumps == true && hasGun)
        {
            isJumping = true;
            isFalling = false;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpforce;
            extraJumps = false;
        }
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpforce;
                jumpTimeCounter -= Time.deltaTime;

            }
            else
            {
                isJumping = false;
                isFalling = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            isFalling = true;
        }
    }
    private void Grounded()
    {
        //isGrounded = Physics2D.OverlapCircle(feetPos.position, radio, suelo);
        Collider2D collider = Physics2D.OverlapCircle(feetPos.position, radio, suelo);
        isGrounded = collider;
        if (collider != null && collider.CompareTag("sueloSeguro"))
        {
            lastPosition = transform.position;
        }
        isFalling = false;
        if (extraJumps == false && isGrounded == true)
        {
            extraJumps = true;
        }
    }
    private void Crouching()
    {

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            BoxCollider2D[] colliders;
            colliders = gameObject.GetComponents<BoxCollider2D>();
            colliders[1].enabled = true;
            colliders[0].enabled = false;
            isCrouching = true;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) || isFalling == true)
        {
            BoxCollider2D[] colliders;
            colliders = gameObject.GetComponents<BoxCollider2D>();
            colliders[0].enabled = true;
            colliders[1].enabled = false;
            isCrouching = false;
        }
    }
    public void Heal()
    {
        if (isCrouching)
        {
            return;
        }
        HealPlayer(50);
        
        ron--;
        hudControl.UpdateRon(ron/maxRon);
    }
    private void HealPlayer(int healAmount)
    {
        if (hp >= maxHp)
        {
            return;
        }
        hp += healAmount;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        hudControl.UpdatePlayerLife(hp/maxHp);
    } 
    private void Traspass()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider2D floor = Physics2D.OverlapCircle(feetPos.position, radio, suelo);
            if (floor.CompareTag("sueloTraspasable"))
            {
                floor.GetComponent<TraspassFloor>().StartCoroutine("Rotar");
                isFalling = true;
                Crouching();
            }
        }
    }
    public void ReplenishRon(int num)
    {
        ron += num;
        if (ron > maxRon)
        {
            ron = maxRon;
        }
        hudControl.UpdateRon(ron/maxRon);
    }
    private void Aim()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            aiming = true;
        }
        if (Input.GetKey(KeyCode.F))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    Distancia(113, 157);
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    Distancia(23, 67);
                }
                else
                {
                    Distancia(68, 112);
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                Distancia(158, 180);
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                Distancia(0, 22);
            }
        }
        if (ganchoCercano != null && Input.GetKeyUp(KeyCode.F))
        {
            StopCoroutine("Gancho");
            StartCoroutine("Gancho");
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            aiming = false;
            if (ganchoCercano!=null)
            {
                ganchoCercano.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            ganchoCercano = null;
        }
    }
    private void Distancia(int minAngle, int maxAngle)
    {
        ganchoCercano = null;
        for (int i = 0; i < ganchos.Count; i++)
        {
            if (ganchos[i] != null)
            {
                Vector2 targetDir = ganchos[i].position - transform.position;
                float angle = Vector2.Angle(targetDir, transform.right);
                if (angle > minAngle && angle < maxAngle)
                {
                    if (ganchoCercano == null)
                    {
                        ganchoCercano = ganchos[i];
                    }
                    else
                    {
                        if (Vector2.Distance(ganchos[i].position, transform.position) < Vector2.Distance(ganchoCercano.position, transform.position))
                        {
                            ganchoCercano = ganchos[i];
                        }
                    }
                }
            }
        }
        RestablecerColorGanchos();
        if (ganchoCercano != null)
        {
            ganchoCercano.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    private void RestablecerColorGanchos()
    {
        for (int i = 0; i < ganchos.Count; i++)
        {
            ganchos[i].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    private IEnumerator Gancho()
    {
        Debug.Log(ganchoCercano);
        isHooking = true;
        canMove = false;
        Transform objetivo = ganchoCercano;
        line.enabled = true;
        line.SetPosition(1, objetivo.position);
        rb.velocity = new Vector2(0, 0);
        rb.bodyType = RigidbodyType2D.Kinematic;
        while (transform.position != objetivo.position)
        {
            line.SetPosition(0, firePosition.position);
            transform.position = Vector2.MoveTowards(transform.position, objetivo.position, speed * Time.deltaTime * 1.5f);
            yield return new WaitForEndOfFrame();
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        line.enabled = false;
        isHooking = false;
        canMove = true;
    }
    public void Loro()
    {
        if (!isGrounded || isCrouching || !hasParrot)
        {
            return;
        }
        Debug.Log("loro");
        if (usingLoro)
        {
            loro.GetComponent<Loro>().DesactivarLoro();
        }
        else
        {
            usingLoro = true;
            loro.SetActive(true);
        }
    }

    public void Attack()
    {
        if (Time.time < lastTimeAttack + attackCooldown || Time.time < lastFinishedCombo + finishComboCooldown || isHooking || usingLoro)
        {
            return;
        }
        canMove = false;
        lastTimeAttack = Time.time;
        attackCounter++;
        if (attackCounter == 1)
        {
            weapon.GetComponent<WeaponController>().damage = damage + addedDamage;
            weapon.GetComponent<SpriteRenderer>().color = Color.yellow;

        }
        else if (attackCounter == 2)
        {
            weapon.GetComponent<WeaponController>().damage += (damage + addedDamage)* 50 / 100;
            weapon.GetComponent<SpriteRenderer>().color = new Color32(250,156,28,255);
        }
        else if (attackCounter == 3)
        {
            weapon.GetComponent<WeaponController>().damage += (damage + addedDamage) * 100 / 100;
            weapon.GetComponent<SpriteRenderer>().color = Color.red;
            attackCounter = 0;
            lastFinishedCombo = Time.time;
        }
        StartCoroutine("AttackAnim");
    }
    IEnumerator AttackAnim()
    {
        weapon.GetComponent<Collider2D>().enabled = true;
        weapon.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        weapon.GetComponent<Collider2D>().enabled = false;
        weapon.SetActive(false);
        canMove = true;
    }
    public void Shoot()
    {
        if (!canShoot || usingLoro || !hasGun)
        {
            return;
        }
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
    public void HurtPlayer(int damage, Vector2 attackPosition, bool isTrap, bool canParry)
    {
        if (!isVulnerable && !isTrap)
        {
            if (parry.activeSelf && canParry)
            {
                float attackDir = attackPosition.x - transform.position.x;
                if (attackDir >= 0)
                {
                    attackDir = 1;
                }
                else
                {
                    attackDir = -1;
                }
                float parryDir = parry.transform.position.x - transform.position.x;
                if (parryDir >= 0)
                {
                    parryDir = 1;
                }
                else
                {
                    parryDir = -1;
                }
                Debug.Log(attackDir + " " + parryDir + " -> " + (attackDir != parryDir));
                if (attackDir != parryDir)
                {
                    Hurt(damage);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
        Hurt(damage);
    }
    IEnumerator HitInvulnerable()
    {
        Debug.Log(Time.time < lastTimeHurt + invulnerableTime);
        while (Time.time < lastTimeHurt + invulnerableTime)
        {
            if (isVulnerable)
            {
                isVulnerable = false;
            }
            if (!GetComponent<SpriteRenderer>().color.Equals(Color.red))
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            yield return new WaitForEndOfFrame();
        }
        if (!parry.activeSelf)
        {
            isVulnerable = true;
        }
        GetComponent<SpriteRenderer>().color = Color.white;

    }
    private void Hurt(int damage)
    {
        Debug.Log(internalDamage);
        hp -= (int)((damage + internalDamage) / defense);
        Debug.Log("PlayerHurt: " + hp);
        if (hp <= 0)
        {
            Dead();
        }
        internalDamage = 0;
        lastTimeHurt = Time.time;
        StartCoroutine("HitInvulnerable");
    }
    public void Rest()
    {
        ron = maxRon;
        hp = maxHp;
        hudControl.UpdatePlayerLife(hp/maxHp);
        hudControl.UpdateRon(ron/maxRon);
    }
    private void Dead()
    {
        Rest();
        Debug.Log(hp);
        PlayerPrefs.SetString("accion", "Respawning");
        SceneManager.LoadScene(PlayerPrefs.GetString("sceneRespawn"));
    }
    IEnumerator Parry()
    {
        lastTimeParry = Time.time;
        canMove = false;
        parry.GetComponent<ParryController>().isPerfect = true;
        parry.GetComponent<SpriteRenderer>().color = Color.green;
        isVulnerable = false;
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
        canMove = true;
    }
    IEnumerator Dodge()
    {
        Quaternion startRot = transform.rotation;
        canMove = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        lastTimeDodge = Time.time;
        isVulnerable = false;
        int dir = GetFacingDirection();
        Debug.Log(GetFacingDirection());
        rb.velocity = Vector3.zero;
        do
        {
            rb.velocity += Vector2.right * (dodgeSpeed * 100 * dir * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        } while (Time.time - lastTimeDodge <= dodgeDuration);
        isVulnerable = true;
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.rotation = startRot;
        canMove = true;
        lastTimeDodge = Time.time;
    }
    public void InternalHurtPlayer(int addInternalDamage)
    {
        StopCoroutine("HealInternalDamage");
        internalDamage += addInternalDamage;
        lastTimeHurt = Time.time;
        isHealingInternalDamage = false;
        hudControl.UpdatePlayerLife(hp-internalDamage/maxHp);
    }
    IEnumerator HealInternalDamage()
    {
        Debug.Log("Player HealingInternalDamage: " + internalDamage);
        isHealingInternalDamage = true;
        while (internalDamage > 0)
        {
            internalDamage--;
            hudControl.UpdatePlayerLife(hp-internalDamage/maxHp);
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Player Finished HealingInternalDamage: " + internalDamage);
        isHealingInternalDamage = false;
        hudControl.UpdatePlayerLife(hp/maxHp);
        
    }
}
