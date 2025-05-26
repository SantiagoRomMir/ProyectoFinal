using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rest : MonoBehaviour
{
    public String  Scene;
    // Start is called before the first frame update    

    private void Awake()
    {
        Scene = SceneManager.GetActiveScene().name;
    }
    private void Start()
    {
        GetComponent<AudioSource>().volume = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>().GetSoundSource().volume;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if(collision.gameObject.CompareTag("Player")){
            
            if(Input.GetKey(KeyCode.UpArrow)){
                PlayerPrefs.SetString("positionRespawn",gameObject.name);
                PlayerPrefs.SetString("sceneRespawn",Scene);
                if (!collision.gameObject.GetComponent<PlayerController>().isResting)
                {
                    collision.gameObject.GetComponent<PlayerController>().isResting = true;
                    collision.gameObject.GetComponent<PlayerController>().StartCoroutine("Rest");
                }
            }
        }
    }
}
