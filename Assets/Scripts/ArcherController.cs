using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.U2D;

public class ArcherController : MonoBehaviour
{
    public bool aggro;
    public GameObject arrow;
    public Vector2 targetPos;
    public float shootDelay;
    public float shootCooldown;
    public bool canShoot;
    public float arowSpeedMultiplier;
    public bool arrowCanBeReflected;
    private void Awake()
    {
        canShoot = true;
    }
    private void Update()
    {
        GetComponent<Animator>().SetFloat("VelocityX", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
        if (aggro && canShoot)
        {
            //Debug.Log("Disparo Flecha");
            StartCoroutine("ShootArrow");
            aggro = false;
        }
    }
    IEnumerator ShootArrow()
    {
        canShoot = false;
        Vector2 target = targetPos;
        GetComponent<EnemyController>().move = false;
        //Debug.Log(transform.position.x - target.x);
        if (transform.position.x - target.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (transform.position.x - target.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(shootDelay);
        InstantiateArrow(target);
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
    private void InstantiateArrow(Vector2 target)
    {
        targetPos = target;
        GameObject shotArrow = Instantiate(arrow, transform);
        shotArrow.GetComponent<ArrowController>().canBeReflected = arrowCanBeReflected;
        shotArrow.GetComponent<ArrowController>().speedMultiplier = arowSpeedMultiplier;
    }
}
