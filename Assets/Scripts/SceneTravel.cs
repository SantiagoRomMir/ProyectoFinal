using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTravel : MonoBehaviour
{
    public String Scene;
    private SceneSelector sceneSelector;
    private void Awake()
    {
        // Todo SceneTravel debe tener un hijo vacio con SceneSelector para que cada uno pueda tener un valor independiente del prefab
        sceneSelector = transform.GetChild(0).GetComponent<SceneSelector>();
        Scene = sceneSelector.GetSelectedScene();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().SavePersistenceData();
            SceneManager.LoadScene(Scene);
        }
    }
}
