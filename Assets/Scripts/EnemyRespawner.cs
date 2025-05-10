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
        enemyCopy = Instantiate(GetRealEnemy(), transform);
        enemyCopy.SetActive(false);
        enemyCopy.transform.position = gameObject.transform.position;
        enemyCopy.tag = "Copy";
    }
    public void SpawnEnemy()
    {
        DestroyRealEnemy();
        GameObject newEnemy = Instantiate(enemyCopy, transform);
        newEnemy.tag = "Untagged";
        newEnemy.SetActive(true);
    }
    public GameObject GetRealEnemy()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).CompareTag("Copy"))
            {
                return transform.GetChild(i).gameObject;
            }
        }
        return null;
    }
    public void DestroyRealEnemy()
    {
        Destroy(GetRealEnemy());
    }
}
