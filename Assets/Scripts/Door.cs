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
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        sceneSelector = GetComponent<SceneSelector>();
        Scene = sceneSelector.GetSelectedScene();
    }
    // Start is called before the first frame update    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (Input.GetKey(KeyCode.UpArrow))
            {
                PlayerPrefs.SetString("accion", "puerta");
                PlayerPrefs.SetString("Door", gameObject.name);
                collision.gameObject.GetComponent<PlayerController>().SavePersistenceData();
                StartCoroutine("Abrir");
            }
        }
    }
    private IEnumerator Abrir()
    {
        anim.Play("Abrir");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(Scene);
    }
}
