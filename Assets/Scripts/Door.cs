using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public String Scene;
    private SceneSelector sceneSelector;
    private void Awake()
    {
        sceneSelector = GetComponent<SceneSelector>();
        Scene = sceneSelector.GetSelectedScene();
    }
    // Start is called before the first frame update    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            
            if(Input.GetKey(KeyCode.UpArrow)){
                PlayerPrefs.SetString("accion","puerta");
                PlayerPrefs.SetString("Door",gameObject.name);
                collision.gameObject.GetComponent<PlayerController>().SavePersistenceData();
                SceneManager.LoadScene(Scene);
            }
        }
    }
}
