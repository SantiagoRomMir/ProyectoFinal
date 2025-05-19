using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAggro : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision!=null && collision.CompareTag("Player"))
        {
            transform.parent.GetComponent<ArcherController>().aggro = true;
            transform.parent.GetComponent<EnemyController>().stop = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            transform.parent.GetComponent<ArcherController>().aggro = false;
            StopCoroutine("ShootArrow");
            transform.parent.GetComponent<EnemyController>().stop = 1;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            transform.parent.GetComponent<ArcherController>().targetPos = collision.transform.position;
        }
    }
}
