using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class ArrowController : MonoBehaviour
{
    public float speed;
    public float speedMultiplier;
    public float lifeTime;
    public int damage;
    public GameObject finalPosTest;
    public Vector2 finalPos;
    public Vector2 direction;
    private float hitTargetTime;
    private bool targetHit;
    private Vector2 targetPos;
    public string hitTag;
    public bool canBeReflected;
    public bool isFromTrap;
    private void Awake()
    {
        hitTag = "Player";
    }
    private void Start()
    {
        if (!isFromTrap)
        {
            finalPos = transform.parent.GetComponent<ArcherController>().targetPos + new Vector2(0, 0);
        }
        targetPos = finalPos;
        direction = finalPos - (Vector2)transform.position;
        hitTargetTime = Time.time;
        targetHit = false;
        float rot_z = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, -rot_z);
    }
    void Update()
    {
        Vector2 pos = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, finalPos, Time.deltaTime*(speed*speedMultiplier));
        finalPos += direction * 1.1f;

        if (Mathf.Abs(transform.position.x - targetPos.x) < 0.5f && !targetHit)
        {
            Debug.Log("HitTarget");
            hitTargetTime = Time.time;
            targetHit = true;
        }
        if (pos.Equals(transform.position) || Time.time >= hitTargetTime + lifeTime && targetHit)
        {
            //Debug.Log(pos.Equals(transform.position));
            //Debug.Log(Time.time >= hitTargetTime + lifeTime && targetHit);
            //Debug.Log("ArrowFinalPos");
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        //Debug.Log(collision.gameObject.tag);
        if (collision != null && collision.CompareTag(hitTag))
        {
            switch (hitTag)
            {
                case "Player":
                    collision.GetComponent<PlayerController>().HurtPlayer(damage, transform.position, false, true);
                    Destroy(gameObject);    
                    break;
                case "Enemy":
                    transform.parent.GetComponent<EnemyController>().HurtEnemy(damage);
                    Destroy(gameObject);
                    break;
            }
        }
        if (collision != null && collision.gameObject.layer == 6)
        {
            //Debug.Log("GroundHit: "+collision.gameObject.name);
            Destroy(gameObject);
        }
    }
    public void DeflectArrow()
    {
        if (!canBeReflected)
        {
            return;
        }
        hitTag = "Enemy";

        targetPos = transform.parent.position;
        finalPos = transform.parent.position;
        direction = finalPos - (Vector2)transform.position;
        hitTargetTime = Time.time;
        targetHit = false;

        float rot_z = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, -rot_z);

        Debug.Log("Deflecting Arrow new TargetPos: " + finalPos);
    }
}
