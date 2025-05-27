using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    public GameObject Trap;
    public int direction;

    public void Start()
    {
        StartCoroutine(SpawnTrap());
    }

    IEnumerator SpawnTrap()
    {
        while (true) {
            GameObject arrow = Instantiate(Trap, transform.position, Quaternion.identity);
            if (Trap.GetComponent<ArrowController>()!=null)
            {
                arrow.GetComponent<ArrowController>().isFromTrap = true;
                arrow.GetComponent<ArrowController>().speedMultiplier = 1f;
                arrow.GetComponent<ArrowController>().canBeReflected = false;
                arrow.GetComponent<ArrowController>().damage = 10;
                arrow.GetComponent<ArrowController>().finalPos = new Vector2(transform.position.x + 100 * direction, transform.position.y);

                //Debug.Log(arrow.GetComponent<ArrowController>().finalPos);
            }
            yield return new WaitForSeconds(Random.Range(3, 7));
        }
    }
}
