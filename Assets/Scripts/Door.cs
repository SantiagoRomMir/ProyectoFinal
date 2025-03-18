using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public String  Scene;
    // Start is called before the first frame update    
    private void OnTriggerStay2D(Collider2D collision)
    {

        if(collision.gameObject.CompareTag("Player")){
            
            if(Input.GetKey(KeyCode.UpArrow)){
                PlayerPrefs.SetString("Door",gameObject.name);
                SceneManager.LoadScene(Scene);
            }
        }
    }
}
