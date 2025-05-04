using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision){
	// comentario desde maquina virtual 
       if(collision.gameObject.CompareTag("Player")){
            collision.gameObject.GetComponent<PlayerController>().HurtPlayer(10, transform.position, true);
            collision.gameObject.transform.position=collision.gameObject.GetComponent<PlayerController>().lastPosition;
        }
    }
}
