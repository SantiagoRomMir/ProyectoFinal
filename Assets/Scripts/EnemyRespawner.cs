using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    public GameObject enemyCopy;
    private GameObject activeEnemy;
    private void Awake()
    {
        CopyEnemy();
    }
    public void SpawnEnemy()
    {
        Destroy(transform.GetChild(0).gameObject);
        CopyEnemy();
        enemyCopy.SetActive(true);
    }

    private void CopyEnemy()
    {
        enemyCopy = Instantiate(transform.GetChild(0).gameObject, transform);
        enemyCopy.SetActive(false);
        enemyCopy.name = transform.GetChild(0).gameObject.name;
    }
}
