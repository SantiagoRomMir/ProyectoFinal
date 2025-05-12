using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

using UnityEngine;

public class Boss : MonoBehaviour
{
    public Canvas canvas;
    private HudControl hudControl;
    public float maxHp;
    public float hp;
    public float speed;
    private Transform[] jumpPositions;
    public Transform[] cañonesEste;
    public Transform[] cañonesOeste;
    private Rigidbody2D phisics;
    public BoxCollider2D colliderAtaque;
    public GameObject bullet;
    private int direction;
    // Start is called before the first frame update
    void Start()
    {
        
        phisics=gameObject.GetComponent<Rigidbody2D>();
        hp=maxHp;
        hudControl=canvas.GetComponent<HudControl>();
        hudControl.ActiveBossBar();
        jumpPositions=GameObject.Find("jumpPositions").transform.GetComponentsInChildren<Transform>();
        StartCoroutine("Arrollar");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Saltar(){
        StopAllCoroutines();
        StartCoroutine("Jump");
    }
    private IEnumerator Jump(){
        
        Transform objetivo = MasCercano();
        if(objetivo.position.x>transform.position.x){
                direction=1;
            }else{
                direction=-1;
            }
        while(math.abs(transform.position.x-objetivo.position.x)>0.1f){
            phisics.velocity = new Vector2(direction * speed, phisics.velocity.y);
            Debug.Log(transform.position.x);
            Debug.Log("objetivo "+objetivo.position.x);
            yield return new WaitForEndOfFrame();
        }
        phisics.velocity = new Vector2(0, 0);
        phisics.bodyType = RigidbodyType2D.Kinematic;
        while(transform.position!=new Vector3(transform.position.x,15,transform.position.z)){
            Debug.Log("hola");
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x,15), speed * Time.deltaTime * 4f);
                    yield return new WaitForEndOfFrame();
        }
        phisics.bodyType = RigidbodyType2D.Dynamic;
    }
    private Transform MasCercano(){
        if(Vector2.Distance(transform.position,jumpPositions[1].position)<Vector2.Distance(transform.position,jumpPositions[2].position)){
            return jumpPositions[1];
        }else{
            return jumpPositions[2];
        }
    }
    private Transform MasLejano(){
        if(Vector2.Distance(transform.position,jumpPositions[1].position)<Vector2.Distance(transform.position,jumpPositions[2].position)){
            return jumpPositions[2];
        }else{
            return jumpPositions[1];
        }
    }
    private IEnumerator Shoot(){
        int Salto=Caer();
        yield return new WaitForSeconds(2);
        if(Salto==2){
            for (int i = 0; i < cañonesEste.Length; i++)
            {

                bullet.GetComponent<BalaCañon>().direction = -1;
                Instantiate(bullet, new Vector2(cañonesEste[i].position.x,cañonesEste[i].position.y),Quaternion.identity);
            }
        }else{
            for (int i = 0; i < cañonesOeste.Length; i++)
            {
                bullet.GetComponent<BalaCañon>().direction = 1;
                Instantiate(bullet, new Vector2(cañonesOeste[i].position.x,cañonesOeste[i].position.y),Quaternion.identity);
            }
        }         

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
    private int Caer(){
         int Salto=UnityEngine.Random.Range(1,3);
        transform.position=jumpPositions[Salto].position;
        return Salto;
    }
    private IEnumerator Arrollar(){
        int Salto=Caer();
        yield return new WaitForSeconds(2);
        Transform objetivo = MasLejano();
        colliderAtaque.enabled=true;
        if(objetivo.position.x>transform.position.x){
                direction=1;
            }else{
                direction=-1;
            }
        while(math.abs(transform.position.x-objetivo.position.x)>0.1f){
            phisics.velocity = new Vector2(direction * speed, phisics.velocity.y);
            yield return new WaitForEndOfFrame();
        }
        Saltar();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            StopCoroutine("Arrollar");
            colliderAtaque.enabled=false;
            phisics.velocity=Vector2.zero;
        }
    }

}
