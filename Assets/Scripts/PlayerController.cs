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

    public float maxHp;
    private float hp;
    public float maxRon;
    private float ron;

    public bool isGrounded;
    private bool isJumping;
    private bool isCrouching;
    private bool extraJumps;
    public bool isFalling;
    private bool aiming;
    public bool usingLoro;
    public float Slowed = 1;

    public float invulnerableTime;

    public float hookSpeed;

    public bool isResting;
    public bool isFastFall;

    [Header("MeleeAttack")]
    public GameObject weapon;
    public int damage;
    public int attackNumberChain;
    public float attackTimeChainReset;
    private float lastTimeAttack;
    public float attackCooldown;
    private float lastFinishedCombo;
    public float finishComboCooldown;
    public int attackCounter;
    private bool isLookingUp;

    [Header("Parry")]
    public GameObject parry;
    private bool isVulnerable;
    public float parryCooldown;
    public float parryDuration;
    private float lastTimeParry;
    public float perfectParryTimeWindow;
    public float parryCancelDelay;
    public float internalDamage;
    private float lastTimeHurt;
    private bool isHealingInternalDamage;
    public float healInternalDamageDelay;
    public bool isParrying;
    private bool parryKeyUp;

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
    public bool canMove;

    [Header("Controls")]
    public KeyCode attackKey;
    public KeyCode parryKey;
    public KeyCode shootKey;
    public KeyCode reloadKey;
    public KeyCode dodgeKey;

    [Header("Sound")]
    private SoundController soundController;

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
    public bool isRegeneratingHealth;
    public List<Consumable> consumables;
    private float lastConsumableTime;

    [Header("Persistence")]
    public bool clearPersistenceData; // If True Ignore Persistent Data and Overwrite it
    private Persistence persistence;

    [Header("Animator")]
    public Animator animator;
    private int idleNumber;

    [Header("Trap")]
    public float movementDirAbs;

    [Header("Simulation")]
    public bool killPlayer;

    [Header("FootSteps")]
    public AudioSource footStepsAudio;
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
        StartCoroutine("SelectRandomIdle");
        animator = GetComponent<Animator>();
        canvas = GameObject.FindGameObjectWithTag("Hud").GetComponent<Canvas>();
        hudControl=canvas.GetComponent<HudControl>();
        consumables = new List<Consumable>();
        selectedConsumable = 0;
        defense = 1f;
        lastConsumableTime = Time.time;

        soundController = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>();
        if (PlayerPrefs.GetString("accion") == "puerta")
        {
            if (GameObject.Find(PlayerPrefs.GetString("Door"))!=null)
            {
                transform.position = GameObject.Find(PlayerPrefs.GetString("Door")).transform.position;
            }
        }
        if (PlayerPrefs.GetString("accion") == "travel")
        {
            if (GameObject.Find(PlayerPrefs.GetString("posicionViaje")) != null)
            {
                transform.position = GameObject.Find(PlayerPrefs.GetString("posicionViaje")).transform.position;
            }
        }
        if (PlayerPrefs.GetString("accion") == "Respawning")
        {
            if (GameObject.Find(PlayerPrefs.GetString("positionRespawn")) != null)
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
        //hp = 10;
        isHooking = false;
        usingLoro = false;
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

        hasHook = false;
        hasGun = false;
        hasParrot = false;

        LoadPersistenceData();

        StartCoroutine("AttackUpwards");
        StartCoroutine("StopParry");
        StartCoroutine("CheckFastFall");

        footStepsAudio = GetComponent<AudioSource>();
        footStepsAudio.volume = soundController.GetSoundSource().volume;
        StartCoroutine("FootStepsSound");

        AddConsumable(new Consumable(0,Consumable.TypeConsumable.Datil));
        AddConsumable(new Consumable(0, Consumable.TypeConsumable.Pera));
        AddConsumable(new Consumable(0, Consumable.TypeConsumable.Hierbabuena));

        UpdateInventory();
    }

    void FixedUpdate()
    {
        if (!usingLoro && canMove && !isResting && !aiming)
        {
            direction = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(direction * speed * Slowed, rb.velocity.y);
            //Debug.Log(direction);
            if (direction < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                if (!isLookingUp)
                {
                    weapon.transform.localPosition = new Vector2(Mathf.Abs(weapon.transform.localPosition.x) * -1, weapon.transform.localPosition.y);
                }
                parry.transform.localPosition = new Vector2(Mathf.Abs(parry.transform.localPosition.x) * -1, parry.transform.localPosition.y);
                firePosition.transform.localPosition = new Vector2(Mathf.Abs(firePosition.transform.localPosition.x) * -1, firePosition.transform.localPosition.y);

            }
            else if (direction > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                if (!isLookingUp)
                {
                    weapon.transform.localPosition = new Vector2(Mathf.Abs(weapon.transform.localPosition.x), weapon.transform.localPosition.y);
                }
                parry.transform.localPosition = new Vector2(Mathf.Abs(parry.transform.localPosition.x), parry.transform.localPosition.y);
                firePosition.transform.localPosition = new Vector2(Mathf.Abs(firePosition.transform.localPosition.x), firePosition.transform.localPosition.y);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!usingLoro){
            if (hasHook && isGrounded)
            {
                Aim();
                if (aiming)
                {
                    canMove = false;
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
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
                if (!isHooking || canMove)
                {
                    Jump();
                }
            }

            if (Input.GetKeyDown(parryKey) && Time.time >= lastTimeParry + parryCooldown && !isResting)
            {
                parryKeyUp = false;
                lastTimeParry = Time.time;
                isParrying = true;
                StartCoroutine("Parry");
            }
            if (Input.GetKeyUp(parryKey))
            {
                parryKeyUp = true;
            }

            UseConsumable();
        }

        if (!isHealingInternalDamage && Time.time > lastTimeHurt + healInternalDamageDelay && internalDamage > 0)
        {
            StartCoroutine("HealInternalDamage");
        }

        CheckAttackCombo();

        UpdateAnimatorValues();

        if (killPlayer)
        {
            killPlayer = false;
            HurtPlayer(10000, transform.position, true, false);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SelectNextConsumable();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SelectPreviousConsumable();
        }

        hudControl.UpdateMoney(money);
        UpdateActiveBuffs();
    }
    IEnumerator FootStepsSound()
    {
        while (true)
        {
            if (Mathf.Abs(rb.velocity.x) > 0f && isGrounded)
            {
                AudioClip sound = soundController.footSteps[UnityEngine.Random.Range(0, soundController.footSteps.Length - 1)];
                footStepsAudio.PlayOneShot(sound);
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
    IEnumerator CheckFastFall()
    {
        while (true)
        {
            if (rb.velocity.y < -20f)
            {
                isFastFall = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator StopParry()
    {
        while (true)
        {
            if (parryKeyUp || (Time.time >= lastTimeParry + parryDuration) && isParrying)
            {
                parryKeyUp = false;
                yield return new WaitForSeconds(parryCancelDelay);
                StopCoroutine("Parry");
                isParrying = false;
                canMove = true;
                isVulnerable = true;
                parry.SetActive(false);
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private void UpdateActiveBuffs()
    {
        hudControl.UpdateActiveBuffs(0, defense!=1);
        hudControl.UpdateActiveBuffs(1, addedDamage != 0);
        hudControl.UpdateActiveBuffs(2, isRegeneratingHealth);
    }
    IEnumerator AttackUpwards()
    {
        float distanceFromPlayer = weapon.transform.localPosition.x;
        //Debug.Log(distanceFromPlayer);
        while (true)
        {
            //Debug.Log(isLookingUp);
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

        AddConsumableList(Persistence.LoadPersistenceInventory());
    }
    private void AddConsumableList(List<Consumable> consumableList)
    {
        foreach (Consumable c in consumableList)
        {
            AddConsumable(c);
        }
    }
    public void SavePersistenceData()
    {
        persistence = new Persistence(hp, ron, internalDamage, selectedConsumable, addedDamage, defense, hasHook, hasParrot, hasGun, canShoot, money);

        persistence.SavePersistence();

        if (GameObject.FindGameObjectWithTag("EnemiesManager")!=null)
        {
            GameObject.FindGameObjectWithTag("EnemiesManager").GetComponent<EnemiesManager>().SaveEnemiesDead();
        }
        if (GameObject.FindGameObjectWithTag("EventsManager") != null)
        {
            GameObject.FindGameObjectWithTag("EventsManager").GetComponent<SingleTimeEventsManager>().SaveVasesBroken();
        }

        Persistence.SavePersistenceInventory(consumables);
    }
    public void AddMoney(int money)
    {
        this.money += money;
    }
    public void AddConsumable(ConsumableController consumable)
    {
        foreach (Consumable c in consumables)
        {
            if (c.consumable.ToString().ToLower().Equals(consumable.consumable.ToString().ToLower()) && c.remainingAmount!=0) // Stack Repeated Consumables
            {
                c.remainingAmount++;
                UpdateInventory();
                return;
            }
        }
        Consumable newConsumable = new Consumable(consumable);
        consumables.Add(newConsumable);

        UpdateInventory();
    }
    public void AddConsumable(Consumable consumable)
    {
        foreach (Consumable c in consumables)
        {
            if (c.consumable.ToString().ToLower().Equals(consumable.consumable.ToString().ToLower()) && c.remainingAmount != 0) // Stack Repeated Consumables
            {
                c.remainingAmount++;
                UpdateInventory();
                return;
            }
        }
        consumables.Add(new Consumable(consumable));

        UpdateInventory();
    }
    public void UseConsumable()
    {
        if (consumables[selectedConsumable].remainingAmount<=0 || isResting)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.G) && Time.time > lastConsumableTime+consumableCooldown)
        {
            AudioClip sound = soundController.consumable[UnityEngine.Random.Range(0, soundController.consumable.Length - 1)];
            soundController.GetSoundSource().PlayOneShot(sound);
            animator.SetTrigger("Consumable");
            lastConsumableTime = Time.time;
            consumables[selectedConsumable].OnUseAction();
            UpdateInventory();
        }
    }
    private void UpdateInventory()
    {
        int leftIndex = selectedConsumable - 1;
        if (leftIndex < 0)
        {
            leftIndex = consumables.Count - 1;
        }
        int rightIndex = selectedConsumable + 1;
        if (rightIndex > consumables.Count-1)
        {
            rightIndex = 0;
        }
        Consumable left = consumables[leftIndex];
        Consumable right = consumables[rightIndex];
        //Debug.Log("Left: " + consumables[leftIndex].consumable);
        //Debug.Log("Selected: " + consumables[selectedConsumable].consumable);
        //Debug.Log("Right: " + consumables[rightIndex].consumable);
        hudControl.ChangeInventoryIcons(consumables[selectedConsumable], left, right);
    }

    public void SelectNextConsumable()
    {
        if (isResting)
        {
            return;
        }
        selectedConsumable++;
        if (selectedConsumable > consumables.Count - 1)
        {
            selectedConsumable = 0;
        }
        UpdateInventory();
    }
    public void SelectPreviousConsumable()
    {
        if (isResting)
        {
            return;
        }
        selectedConsumable--;
        if (selectedConsumable < 0)
        {
            selectedConsumable = consumables.Count - 1;
        }
        UpdateInventory();
    }
    public void StartHpRegen(float duration)
    {
        isRegeneratingHealth = true;
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
        isRegeneratingHealth = false;
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
        if (canShoot || isReloading || !hasGun || isResting)
        {
            return;
        }
        StartCoroutine("Reload");
    }
    public void StartDodge()
    {
        if (Time.time <= lastTimeDodge + dodgeCooldown || isHooking || isResting || Slowed<1 || isCrouching || aiming || !isGrounded)
        {
            return;
        }
        AudioClip sound = soundController.dodges[UnityEngine.Random.Range(0, soundController.dodges.Length - 1)];
        soundController.GetSoundSource().PlayOneShot(sound);
        animator.SetBool("isDodging", true);
        animator.SetTrigger("Dash");
        StartCoroutine("Dodge");
    }
    private void Jump()
    {
        if (isResting)
        {
            return;
        }
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            soundController.GetSoundSource().PlayOneShot(soundController.jump);
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpforce;
        }
        else if (isGrounded == false && Input.GetKeyDown(KeyCode.Space) && extraJumps == true && hasGun)
        {
            soundController.GetSoundSource().PlayOneShot(soundController.shoot);
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
        if (isGrounded && isFastFall)
        {
            isFastFall = false;
            soundController.GetSoundSource().PlayOneShot(soundController.playerLand);
        }
        if (collider != null && collider.CompareTag("sueloSeguro"))
        {
            lastPosition = transform.position;
            if(direction < 0)
            {
                movementDirAbs = -1;
            } else
            {
                movementDirAbs = 1;
            }
        }
        isFalling = false;
        if (extraJumps == false && isGrounded == true)
        {
            extraJumps = true;
        }
    }
    private void Crouching()
    {
        if (isResting)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            BoxCollider2D[] colliders;
            colliders = gameObject.GetComponents<BoxCollider2D>();
            colliders[1].enabled = true;
            colliders[0].enabled = false;
            isCrouching = true;
            canMove = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) || isFalling == true)
        {
            BoxCollider2D[] colliders;
            colliders = gameObject.GetComponents<BoxCollider2D>();
            colliders[0].enabled = true;
            colliders[1].enabled = false;
            isCrouching = false;
            canMove = true;
        }
    }
    public void Heal()
    {
        if (isCrouching || !canMove || !isGrounded || isResting || !(ron > 0) || hp == maxHp)
        {
            return;
        }
        soundController.GetSoundSource().PlayOneShot(soundController.heal);

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
        animator.SetTrigger("Heal");
        hp += healAmount;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        hudControl.UpdatePlayerLife(hp/maxHp);
    } 
    private void Traspass()
    {
        if (isResting)
        {
            return;
        }
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
        if (isResting)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            aiming = true;
            soundController.GetSoundSource().PlayOneShot(soundController.aimHook);
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
            canMove = true;
            if (ganchoCercano!=null)
            {
                ganchoCercano.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                soundController.GetSoundSource().PlayOneShot(soundController.useHook);
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
        //Debug.Log(ganchoCercano);
        isHooking = true;
        canMove = false;
        Transform objetivo = ganchoCercano;
        line.enabled = true;
        line.SetPosition(1, objetivo.position);
        rb.velocity = new Vector2(0, 0);
        rb.bodyType = RigidbodyType2D.Kinematic;
        float distance;
        while ((distance = Vector2.Distance(transform.position, objetivo.position))>0.5f)
        {
            line.SetPosition(0, firePosition.position);
            transform.position = Vector2.MoveTowards(transform.position, objetivo.position, speed * Time.deltaTime * (hookSpeed + 1 * Vector2.Distance(transform.position,objetivo.position)));
            yield return new WaitForEndOfFrame();
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        line.enabled = false;
        isHooking = false;
        canMove = true;
    }
    public void Loro()
    {
        if (!isGrounded || isCrouching || !hasParrot || isResting)
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
    private void UpdateAnimatorValues()
    {
        animator.SetInteger("IdleNumber",idleNumber);
        //animator.SetTrigger("Idle");
        animator.SetBool("isAiming", aiming);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("VelocityX", Math.Abs(rb.velocity.x));
        animator.SetFloat("VelocityY", rb.velocity.y);
        animator.SetBool("isHooking", isHooking);
        animator.SetBool("ExtraJump", extraJumps);
        animator.SetBool("isCrouching", isCrouching);
        animator.SetBool("isAttackUp", isLookingUp);
        animator.SetBool("isParrying", isParrying);
        animator.SetBool("isResting", isResting);
    }
    IEnumerator SelectRandomIdle()
    {
        while (true)
        {
            idleNumber = UnityEngine.Random.Range(1, 5);
            yield return new WaitForSeconds(3f);
        }
    }
    public void Attack()
    {
        if (Time.time < lastTimeAttack + attackCooldown || Time.time < lastFinishedCombo + finishComboCooldown || isHooking || usingLoro || isResting)
        {
            return;
        }
        
        attackCounter++;
        animator.SetInteger("AttackNumber", attackCounter);
        animator.SetTrigger("Attack");
        canMove = false;
        lastTimeAttack = Time.time;
        if (!isGrounded)
        {
            soundController.GetSoundSource().PlayOneShot(soundController.attacks[3]);
        }
        if (attackCounter == 1)
        {
            if (isGrounded) soundController.GetSoundSource().PlayOneShot(soundController.attacks[0]);
            weapon.GetComponent<WeaponController>().damage = damage + addedDamage;
            weapon.GetComponent<SpriteRenderer>().color = Color.yellow;

        }
        else if (attackCounter == 2)
        {
            if (isGrounded) soundController.GetSoundSource().PlayOneShot(soundController.attacks[1]);
            weapon.GetComponent<WeaponController>().damage += (damage + addedDamage)* 50 / 100;
            weapon.GetComponent<SpriteRenderer>().color = new Color32(250,156,28,255);
        }
        else if (attackCounter == 3)
        {
            if (isGrounded) soundController.GetSoundSource().PlayOneShot(soundController.attacks[2]);
            weapon.GetComponent<WeaponController>().damage += (damage + addedDamage) * 100 / 100;
            weapon.GetComponent<SpriteRenderer>().color = Color.red;
            attackCounter = 0;
            lastFinishedCombo = Time.time;
        }
        StartCoroutine("AttackAnim");
    }
    IEnumerator AttackAnim()
    {
        if (rb.velocity.x <= 1f)
        {
            rb.velocity += new Vector2(2f * GetFacingDirection(), 0);
        }
        yield return new WaitForSeconds(0.1f);
        weapon.GetComponent<Collider2D>().enabled = true;
        weapon.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        weapon.GetComponent<Collider2D>().enabled = false;
        weapon.SetActive(false);
        canMove = true;
    }
    public void Shoot()
    {
        if (!canShoot || usingLoro || !hasGun || isResting)
        {
            return;
        }
        soundController.GetSoundSource().PlayOneShot(soundController.shoot);
        animator.SetTrigger("Shoot");
        canShoot = false;
        canMove = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
        Invoke("CreateBullet", 0.25f);
    }
    private void CreateBullet()
    {
        canMove = true;
        float forwardDir = GetFacingDirection();
        if (forwardDir == -1)
        {
            bulletPrefab.GetComponent<SpriteRenderer>().flipX = true;
        } else
        {
            bulletPrefab.GetComponent<SpriteRenderer>().flipX = false;
        }
        bulletPrefab.GetComponent<BulletController>().direction = forwardDir;
        Instantiate(bulletPrefab, new Vector2(transform.position.x + forwardDir, transform.position.y+0.45f), Quaternion.identity);
        hudControl.ActiveGunPowder();
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
        animator.SetTrigger("Reload");
        isReloading = true;
        yield return new WaitForSeconds(0.5f);
        soundController.GetSoundSource().PlayOneShot(soundController.gunPowder);
        yield return new WaitForSeconds(reloadTime-0.5f);
        canShoot = true;
        isReloading = false;
        hudControl.ActiveGunPowder();
        soundController.GetSoundSource().PlayOneShot(soundController.gunClick);
    }
    private void CheckAttackCombo()
    {
        if (Time.time > lastTimeAttack + attackTimeChainReset && attackCounter > 0)
        {
            //Debug.Log("ResetCombo");
            attackCounter = 0;
            animator.SetInteger("AttackNumber", attackCounter);
        }
    }
    public void HurtPlayer(int damage, Vector2 attackPosition, bool isTrap, bool canParry)
    {
        if (isResting)
        {
            return;
        }
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
                //Debug.Log(attackDir + " " + parryDir + " -> " + (attackDir != parryDir));
                if (attackDir != parryDir)
                {
                    soundController.GetSoundSource().PlayOneShot(soundController.playerHurt);
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
        if (!isTrap)
        {
            soundController.GetSoundSource().PlayOneShot(soundController.playerHurt);
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
        //Debug.Log(internalDamage);
        hp -= (int)((damage + internalDamage) / defense);
        //Debug.Log("PlayerHurt: " + hp);
        if (hp <= 0)
        {
            animator.SetTrigger("Death");
            StartCoroutine("Dead");
        }
        internalDamage = 0;
        hudControl.UpdatePlayerLife(hp / maxHp);
        Debug.Log(hp - internalDamage / maxHp);
        hudControl.UpdateInternalDamage(hp / maxHp);
        lastTimeHurt = Time.time;
        StartCoroutine("HitInvulnerable");
    }
    IEnumerator Rest()
    {

        hudControl.FadeToBlack();

        isVulnerable = false;
        isResting = true;
        canMove = false;

        rb.velocity = new Vector2(0,rb.velocity.y);

        ron = maxRon;
        hp = maxHp;
        internalDamage = 0;
        hudControl.UpdatePlayerLife(hp/maxHp);
        hudControl.UpdateRon(ron/maxRon);

        if (GameObject.FindGameObjectWithTag("EnemiesManager")!=null)
        {
            GameObject.FindGameObjectWithTag("EnemiesManager").GetComponent<EnemiesManager>().RespawnAllEnemies();
        }

        SavePersistenceData();
        yield return new WaitForSeconds(1f);
        hudControl.FadeToAlpha();
        yield return new WaitForSeconds(1f);

        isResting = false;
        canMove = true;

        isVulnerable = true;
    }
    IEnumerator Dead()
    {
        soundController.GetSoundSource().PlayOneShot(soundController.playerDeath);
        animator.SetBool("isDead", true);
        isVulnerable = false;
        hp = 0;
        internalDamage = 0;
        isResting = true;
        rb.velocity = new Vector2(0, rb.velocity.y);
        //Debug.Log(hp);
        yield return new WaitForSeconds(2f);
        animator.SetBool("isDead", false);
        PlayerPrefs.SetString("accion", "Respawning");
        SceneManager.LoadScene(PlayerPrefs.GetString("sceneRespawn"));
    }
    IEnumerator Parry()
    {
        soundController.GetSoundSource().PlayOneShot(soundController.attacks[1]);
        rb.velocity = new Vector2(0,rb.velocity.y);
        canMove = false;
        yield return new WaitForSeconds(0.1f);
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
        isParrying = false;
    }
    IEnumerator Dodge()
    {
        Quaternion startRot = transform.rotation;
        canMove = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        lastTimeDodge = Time.time;
        isVulnerable = false;
        int dir = GetFacingDirection();
        //Debug.Log(GetFacingDirection());
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
        animator.SetBool("isDodging", false);
    }
    public void InternalHurtPlayer(int addInternalDamage)
    {
        lastTimeHurt = Time.time;
        StopCoroutine("HealInternalDamage");
        isHealingInternalDamage = false;
        hudControl.UpdatePlayerLife((hp - internalDamage) / maxHp);
        if (internalDamage >= hp)
        {
            return;
        }
        internalDamage += addInternalDamage;
    }
    IEnumerator HealInternalDamage()
    {
        Debug.Log("Player HealingInternalDamage: " + internalDamage);
        isHealingInternalDamage = true;
        while (internalDamage > 0)
        {
            internalDamage--;
            hudControl.UpdatePlayerLife((hp - internalDamage) / maxHp);
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Player Finished HealingInternalDamage: " + internalDamage);
        isHealingInternalDamage = false;
        hudControl.UpdatePlayerLife(hp/maxHp);
        
    }
}
