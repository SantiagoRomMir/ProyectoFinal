using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    public GameObject Trap;

    public void Awake()
    {
        StartCoroutine(SpawnTrap());
    }

    IEnumerator SpawnTrap()
    {
        Instantiate(Trap, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(3, 7));    
        StartCoroutine(SpawnTrap());
    }
}
