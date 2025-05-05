using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class Conversation : MonoBehaviour
{
    public GameObject conversacion;
    private List<Transform> mensajes; 
    // Start is called before the first frame update
    void Start()
    {
        
        mensajes=conversacion.transform.Cast<Transform>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerStay2D(Collider2D collision)
    {
       if(collision.gameObject.CompareTag("Player")){
            
            if(Input.GetKey(KeyCode.UpArrow)){
                Time.timeScale=0;
                conversacion.SetActive(true);
                conversacion.transform.GetChild(0).gameObject.SetActive(true);
                StartCoroutine("conversar");
            }
        }
    }
    IEnumerator conversar(){
        int i=1;
        for(;i<conversacion.transform.childCount;++i){
            yield return new WaitUntil(CondicionDeDialogo);
                
                conversacion.transform.GetChild(i-1).gameObject.SetActive(false);
                conversacion.transform.GetChild(i).gameObject.SetActive(true);
                
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(CondicionDeDialogo);
        conversacion.transform.GetChild(i-1).gameObject.SetActive(false);
        conversacion.SetActive(false);
        Time.timeScale=1;
    }
    bool CondicionDeDialogo(){
        return Input.GetKeyDown(KeyCode.P);
    }
}
