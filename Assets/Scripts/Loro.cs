using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loro : MonoBehaviour
{
    public float speed;
    private float directionX;
    private float directionY;
    private Rigidbody2D rb;
    public GameObject Player;
    private PlayerController playerController;
    public float maxDistanceY;
    public float maxDistanceX;
    // Start is called before the first frame update
    void Awake()
    {
        playerController=Player.GetComponent<PlayerController>();
        rb=gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
            directionX = Input.GetAxis("Horizontal");
            directionY =Input.GetAxis("Vertical");
                rb.velocity = new Vector2(directionX * speed, directionY*speed);
                if (directionX < 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (directionX > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
        if(Mathf.Abs(transform.position.y-Player.transform.position.y)>maxDistanceY || Mathf.Abs(transform.position.x-Player.transform.position.x)>maxDistanceX){
            DesactivarLoro();
        }
    }
    public void DesactivarLoro(){
        transform.position=Player.transform.position;
        playerController.usingLoro=false;
        gameObject.SetActive(false);
    }
}
