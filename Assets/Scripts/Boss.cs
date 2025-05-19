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
    public Transform[] cañonesBombardeo;
    public Transform posicionBombardeo;
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
        StartCoroutine("Bombardeo");
    }

    // Update is called once per frame
    private void PatronRandom(){
        switch (UnityEngine.Random.Range(0,3)){
            case 0:
                StartCoroutine("Arrollar");
                break;
            case 1:
                StartCoroutine("Shoot");
                break;
            case 2:
                StartCoroutine("Bombardeo");
                break;  
        }
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
        FlipX(direction);
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
        
        PatronRandom();
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
    private void FlipX(int direction)
    {
        if (direction>0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX=false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX=true;
        }
    }
    private int Caer(){
         int Salto=UnityEngine.Random.Range(1,3);
        transform.position=jumpPositions[Salto].position;
        phisics.bodyType = RigidbodyType2D.Dynamic;
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
        FlipX(direction);
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
    private IEnumerator Bombardeo(){
        Caer();
        yield return new WaitForSeconds(2);
        phisics.velocity = new Vector2(0, 0);
        phisics.bodyType = RigidbodyType2D.Kinematic;
        while(transform.position!=posicionBombardeo.position){
            transform.position = Vector2.MoveTowards(transform.position, posicionBombardeo.position, speed * Time.deltaTime * 4f);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i <4 ;i++)
        {
            int patron=UnityEngine.Random.Range(0,5);
            switch(patron){
                case 0:
                for (int x = 0; x < 6; x++)
                {
                    Instantiate(bullet, new Vector2(cañonesBombardeo[x].position.x,cañonesBombardeo[x].position.y),Quaternion.identity);
                    x++;
                }
                    yield return new WaitForSeconds(5);
                for (int x = 1; x < 6; x++)
                {
                    Instantiate(bullet, new Vector2(cañonesBombardeo[x].position.x,cañonesBombardeo[x].position.y),Quaternion.identity);
                    x++;
                }    
                    break;
                case 1:
                    for (int x = 0; x < 3; x++)
                    {
                        Instantiate(bullet, new Vector2(cañonesBombardeo[x].position.x,cañonesBombardeo[x].position.y),Quaternion.identity);

                    }
                    yield return new WaitForSeconds(5);
                    for (int x = 3; x < 6; x++)
                    {
                        Instantiate(bullet, new Vector2(cañonesBombardeo[x].position.x,cañonesBombardeo[x].position.y),Quaternion.identity); 
                    }
                    break;
                case 2:
                    Instantiate(bullet, new Vector2(cañonesBombardeo[0].position.x,cañonesBombardeo[0].position.y),Quaternion.identity);
                    Instantiate(bullet, new Vector2(cañonesBombardeo[1].position.x,cañonesBombardeo[1].position.y),Quaternion.identity);
                    Instantiate(bullet, new Vector2(cañonesBombardeo[4].position.x,cañonesBombardeo[4].position.y),Quaternion.identity);
                    Instantiate(bullet, new Vector2(cañonesBombardeo[5].position.x,cañonesBombardeo[5].position.y),Quaternion.identity);
                    yield return new WaitForSeconds(5);
                    Instantiate(bullet, new Vector2(cañonesBombardeo[2].position.x,cañonesBombardeo[2].position.y),Quaternion.identity);
                    Instantiate(bullet, new Vector2(cañonesBombardeo[3].position.x,cañonesBombardeo[3].position.y),Quaternion.identity);
                    break;
                case 4:
                    for (int x = 0; x <5;x++)
                    {
                        Instantiate(bullet, new Vector2(cañonesBombardeo[x].position.x,cañonesBombardeo[x].position.y),Quaternion.identity);
                        yield return new WaitForSeconds(0.5f);

                    }
                    yield return new WaitForSeconds(1.5f);
                    for (int x = 5; x > 0; x--)
                    {
                        Instantiate(bullet, new Vector2(cañonesBombardeo[x].position.x,cañonesBombardeo[x].position.y),Quaternion.identity);
                        yield return new WaitForSeconds(0.5f);
                    }
                    break;
            }
            yield return new WaitForSeconds(5);
        }
        phisics.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(2);
        Saltar();
    }

}
