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
    public bool canShoot;
    public float arowSpeedMultiplier;
    public bool arrowCanBeReflected;
    private void Awake()
    {
        canShoot = true;
    }
    private void Update()
    {
        if (aggro && canShoot)
        {
            Debug.Log("Disparo Flecha");
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
