using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public void RespawnAllEnemies()
    {
        for (int i=0; i<transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<EnemyRespawner>().SpawnEnemy();
        }
    }
}
