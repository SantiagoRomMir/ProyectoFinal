using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSlow : MonoBehaviour
{
    public float slowPower;
    private void OnTriggerEnter2D(Collider2D collision){
	// comentario desde maquina virtual 
       if(collision.gameObject.CompareTag("Player")){
            collision.gameObject.GetComponent<PlayerController>().Slowed=slowPower;
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
	// comentario desde maquina virtual 
       if(collision.gameObject.CompareTag("Player")){
            collision.gameObject.GetComponent<PlayerController>().Slowed=1;
        }
    }
}
