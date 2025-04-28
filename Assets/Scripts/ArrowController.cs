using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class ArrowController : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public int damage;
    public GameObject finalPosTest;
    private Vector2 finalPos;
    private Vector2 direction;
    private float hitTargetTime;
    private bool targetHit;
    private Vector2 targetPos;
    private void Awake()
    {
        finalPos = transform.parent.GetComponent<ArcherController>().targetPos + new Vector2(0, 0);
        targetPos = finalPos;
        direction = finalPos - (Vector2)transform.position;
        hitTargetTime = Time.time;
        targetHit = false;
    }
    private void Start()
    {
        float rot_z = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, -rot_z);
    }
    void Update()
    {
        Vector2 pos = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, finalPos, Time.deltaTime*speed);
        finalPos += direction * 1.1f;

        if (Mathf.Abs(transform.position.x - targetPos.x) < 0.5f && !targetHit)
        {
            Debug.Log("HitTarget");
            hitTargetTime = Time.time;
            targetHit = true;
        }
        if (pos.Equals(transform.position) || Time.time >= hitTargetTime + lifeTime && targetHit)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().HurtPlayer(damage);
            Destroy(gameObject);
        }
        if (collision != null && collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
}
