using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryController : MonoBehaviour
{
    public int internalDamage;
    public bool isPerfect;
    public int internalDamagePlayerPercent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("EnemyAttack"))
        {
            if (isPerfect)
            {
                GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().GetSoundSource().PlayOneShot(GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().parry);
                collision.transform.parent.GetComponent<EnemyController>().InternalHurtEnemy(internalDamage);
                //Debug.Log("Perfect Parry");
            }
            else
            {
                GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().GetSoundSource().PlayOneShot(GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().parryPartial);
                transform.parent.GetComponent<PlayerController>().InternalHurtPlayer(internalDamage * internalDamagePlayerPercent / 100);
                //Debug.Log("Partial Parry");
            }
        }
        if (collision != null && collision.CompareTag("Arrow"))
        {
            if (isPerfect)
            {
                GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().GetSoundSource().PlayOneShot(GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().parry);
                collision.GetComponent<ArrowController>().DeflectArrow();
                //Debug.Log("Perfect Parry");
            }
            else
            {
                GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().GetSoundSource().PlayOneShot(GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().parryPartial);
                transform.parent.GetComponent<PlayerController>().InternalHurtPlayer(internalDamage * internalDamagePlayerPercent / 100);
                //Debug.Log("Partial Parry");
            }
        }
    }
}
