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
    private SoundController soundController;
    private void Awake()
    {
        canShoot = true;
    }
    private void Start()
    {
        soundController = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>();
    }
    private void Update()
    {
        if (aggro && canShoot)
        {
            //Debug.Log("Disparo Flecha");
            StartCoroutine("ShootArrow");
            aggro = false;
        }
        if (GetComponent<EnemyController>().health <= 0)
        {
            StopAllCoroutines();
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
        PlayArrowSound();
        targetPos = target;
        GameObject shotArrow = Instantiate(arrow, transform);
        shotArrow.GetComponent<ArrowController>().canBeReflected = arrowCanBeReflected;
        shotArrow.GetComponent<ArrowController>().speedMultiplier = arowSpeedMultiplier;
    }
    private void PlayArrowSound()
    {
        AudioClip sound;
        if (arrowCanBeReflected)
        {
             sound = soundController.bows[Random.Range(0, soundController.bows.Length - 1)];
        }
        else
        {
            sound = soundController.crossbows[Random.Range(0, soundController.crossbows.Length - 1)];
        }
        soundController.GetSoundSource().PlayOneShot(sound);
    }
}
