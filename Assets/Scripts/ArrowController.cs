using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class ArrowController : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public GameObject finalPosTest;
    private Vector2 finalPos;
    private Vector2 direction;
    private float hitTargetTime;
    private bool wayToTarget;
    private void Awake()
    {
        finalPos = finalPosTest.transform.position + new Vector3(0,0,0);
        direction = finalPos - (Vector2)transform.position;
        Debug.Log(direction);
        hitTargetTime = Time.time;
        wayToTarget = true;
    }
    private void Start()
    {
        float rot_z = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Abs(rot_z) + 90);
        }
        else
        {
            rot_z -= 180;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Abs(rot_z) - 90);
        }
    }
    void Update()
    {
        Vector2 pos = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, finalPos, Time.deltaTime*speed);
        finalPos += direction * 1.1f;
        if (Vector2.Distance(transform.position, finalPos) > 0.1f && wayToTarget)
        {
            hitTargetTime = Time.time;
            wayToTarget = false;
        }
        if (pos.Equals(transform.position) /*|| Time.time >= hitTargetTime + lifeTime*/)
        {
            Destroy(gameObject);
        }
    }
}
