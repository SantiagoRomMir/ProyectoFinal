using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject aim;
    public GameObject loro;
    private Rigidbody2D rb;
    private LineRenderer line;
    public List<Transform> ganchos;
    public float speed;
    private float direction;
    private float verticalDirection;

    public Transform feetPos;
    public Transform firePosition;
    private Transform ganchoCercano;

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
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        ron = maxRon;
        hp = maxHp;
        hp = 10;
    }

    void FixedUpdate()
    {
        if(!usingLoro){ 
            if (!aiming)
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
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(!usingLoro){
            Aim();
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
                Jump();
            }
            if (isGrounded && !isCrouching && Input.GetKeyDown(KeyCode.LeftControl) && hp < maxHp)
            {
                Heal();
            }
            if(isGrounded && !isCrouching && Input.GetKeyDown(KeyCode.Q)){
                Loro();
            }
        }
    }
    private void Jump()
    {
        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpforce;
        }
        else if (isGrounded == false && Input.GetKeyDown(KeyCode.Space) && extraJumps == true)
        {
            isJumping = true;
            isFalling = false;
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
        isGrounded = Physics2D.OverlapCircle(feetPos.position, radio, suelo);
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
    private void Heal()
    {
        hp += 50;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        ron--;
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
    }
    private void Aim()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            aiming = true;
        }
        if (Input.GetKey(KeyCode.F))
        {
            if (Input.GetKey(KeyCode.UpArrow) && Input.GetAxisRaw("Horizontal") != 0)
            {
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    Distancia(113, 157);
                    if (ganchoCercano != null)
                    {
                        ganchoCercano.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
                else
                {
                    Distancia(23, 67);
                    if (ganchoCercano != null)
                    {
                        ganchoCercano.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                Distancia(68, 112);
                if (ganchoCercano != null)
                {
                    ganchoCercano.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                Distancia(158, 180);
                if (ganchoCercano != null)
                {
                    ganchoCercano.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                Distancia(0, 22);
                if (ganchoCercano != null)
                {
                    ganchoCercano.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }
        if (ganchoCercano != null && Input.GetKeyDown(KeyCode.E))
        {
            StopCoroutine("Gancho");
            StartCoroutine("Gancho");
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            aiming = false;

            ganchoCercano.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
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
        Transform objetivo = ganchoCercano;
        line.enabled = true;
        line.SetPosition(1, objetivo.position);
        rb.bodyType = RigidbodyType2D.Kinematic;
        while (transform.position != objetivo.position)
        {
            line.SetPosition(0, firePosition.position);
            transform.position = Vector2.MoveTowards(transform.position, objetivo.position, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        line.enabled = false;


    }
    private void Loro(){
        usingLoro=true;
        loro.SetActive(true);
    }

}
