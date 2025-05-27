using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemiesManager : MonoBehaviour
{
    public List<int> enemiesDead;
    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().clearPersistenceData)
        {
            enemiesDead = new List<int>();
            Persistence.SavePersistenceEnemiesDead(SceneManager.GetActiveScene().name, enemiesDead);
        }
        else
        {
            LoadEnemiesDead();

            DestroyEnemiesDead();
        }
    }
    public void RespawnAllEnemies()
    {
        for (int i=0; i<transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<EnemyRespawner>().SpawnEnemy();
        }
    }
    private void GetEnemiesDead()
    {
        enemiesDead = new List<int>();
        for (int i=0; i<transform.childCount; i++)
        {
            //Debug.Log(transform.childCount+" "+ transform.GetChild(i).GetComponent<EnemyRespawner>());
            if (transform.GetChild(i).GetComponent<EnemyRespawner>().GetRealEnemy()==null)
            {
                enemiesDead.Add(i);
            }
        }
    }
    private void DestroyEnemiesDead()
    {
        foreach (int i in enemiesDead)
        {
            transform.GetChild(i).GetComponent<EnemyRespawner>().DestroyRealEnemy();
        }
    }
    public void LoadEnemiesDead()
    {
        enemiesDead = Persistence.LoadPersistenceEnemiesDead(SceneManager.GetActiveScene().name);
        DestroyEnemiesDead();
        // Lee los enemigos muertos almacenados en persistencia
    }
    public void SaveEnemiesDead()
    {
        GetEnemiesDead();
        string tmp = "";
        foreach (int i in enemiesDead) tmp += i + " ";
        //Debug.Log(SceneManager.GetActiveScene().name + ":" + tmp);
        Persistence.SavePersistenceEnemiesDead(SceneManager.GetActiveScene().name, enemiesDead);
    }
}
