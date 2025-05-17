using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    public bool aggro;
    public GameObject arrow;
    public Vector2 targetPos;
    public float shootDelay;
    public float shootCooldown;
    private bool canShoot;
    private void Awake()
    {
        canShoot = true;
    }
    private void Update()
    {
        if (aggro && canShoot)
        {
            StartCoroutine("ShootArrow");
            aggro = false;
        }
    }
    IEnumerator ShootArrow()
    {
        canShoot = false;
        Vector2 target = targetPos;
        yield return new WaitForSeconds(shootDelay);
        InstantiateArrow(target);
        yield return new WaitForSeconds(shootCooldown);
        aggro = true;
        canShoot = true;
    }
    private void InstantiateArrow(Vector2 target)
    {
        targetPos = target;
        Instantiate(arrow, transform);
    }
}
