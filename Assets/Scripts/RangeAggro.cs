using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAggro : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision!=null && collision.CompareTag("Player"))
        {
            StartCoroutine(Aggro(collision.gameObject));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            StopCoroutine("Aggro");
            transform.parent.GetComponent<ArcherController>().aggro = false;
            transform.parent.GetComponent<ArcherController>().StopCoroutine("ShootArrow");
            transform.parent.GetComponent<ArcherController>().canShoot = true;
            transform.parent.GetComponent<EnemyController>().stop = 1;
            transform.parent.GetComponent<EnemyController>().move = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            //Debug.Log("Stay");
            transform.parent.GetComponent<ArcherController>().targetPos = collision.transform.position;
            if (transform.parent.GetComponent<ArcherController>().canShoot)
            {
                transform.parent.GetComponent<ArcherController>().aggro = true;
            }
        }
    }
    IEnumerator Aggro(GameObject g)
    {
        yield return new WaitForSeconds(1f);
        transform.parent.GetComponent<ArcherController>().targetPos = g.transform.position;
        transform.parent.GetComponent<ArcherController>().aggro = true;
        transform.parent.GetComponent<EnemyController>().stop = 0;
    }
}
