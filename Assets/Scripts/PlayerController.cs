using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private float direction;
    private float verticalDirection;
    
    public Transform feetPos;
    public float radio;
    public LayerMask suelo;
    public float jumpforce;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool isGrounded;
    private bool isJumping;
    private bool isCrouching;
    private bool extraJumps;
    private bool isFalling;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
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
        Grounded();
        if(isGrounded){
            Crouching();
        }
        if(isCrouching){
            Traspass();
        }else{
            Jump();
        }
        
        
    }
    private void Jump(){
        if(isGrounded==true && Input.GetKeyDown(KeyCode.Space)){
            isJumping=true;
            jumpTimeCounter=jumpTime;
            rb.velocity=Vector2.up*jumpforce;
        }else if(isGrounded==false && Input.GetKeyDown(KeyCode.Space) && extraJumps==true){
            isJumping=true;
            isFalling=false;
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
                isFalling=true;
            }
        }
        if(Input.GetKeyUp(KeyCode.Space)){
            isJumping=false;
            isFalling=true;
        }
    }
    private void Grounded(){
        isGrounded=Physics2D.OverlapCircle(feetPos.position,radio,suelo);
        isFalling=false;
        if(extraJumps==false && isGrounded==true){
            extraJumps=true;
        }
    }
    private void Crouching(){
       
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            BoxCollider2D[] colliders;
            colliders=gameObject.GetComponents<BoxCollider2D>();
            colliders[1].enabled=true;
            colliders[0].enabled=false;
            isCrouching=true;
        }
        
        if(Input.GetKeyUp(KeyCode.DownArrow) || isFalling==true){
            BoxCollider2D[] colliders;
            colliders=gameObject.GetComponents<BoxCollider2D>();
            colliders[0].enabled=true;
            colliders[1].enabled=false;
            isCrouching=false;
        }
    }
    private void Traspass(){
        if(Input.GetKeyDown(KeyCode.Space)){
            Collider2D floor=Physics2D.OverlapCircle(feetPos.position,radio,suelo);
            if(floor.CompareTag("sueloTraspasable")){
                floor.GetComponent<TraspassFloor>().StartCoroutine("Rotar");
                isFalling=true;
                Crouching();
            }
        }
    }
}
