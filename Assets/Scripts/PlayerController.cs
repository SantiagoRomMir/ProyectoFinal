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
}
