using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrap : MonoBehaviour
{
    private PlayerController player;
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision){
	// comentario desde maquina virtual 
       if(collision!=null && collision.gameObject.CompareTag("Player")){
            Debug.Log("Trampa");
            collision.gameObject.GetComponent<PlayerController>().HurtPlayer(damage,transform.position, true, false);
            float safePosModifier = collision.GetComponent<PlayerController>().movementDirAbs;
            Vector3 savePos = collision.gameObject.GetComponent<PlayerController>().lastPosition;
            //Debug.Log(savePos);
            //Debug.Log(safePosModifier);
            Vector3 newPlayerPos = new Vector3(savePos.x + -safePosModifier, savePos.y+2);
            while (Physics2D.Raycast(newPlayerPos, Vector3.up))
            {
                newPlayerPos = new Vector3(newPlayerPos.x, newPlayerPos.y+1);
            }
            Debug.Log("Y Segura: " + savePos.y);
            collision.gameObject.transform.position = newPlayerPos;
            player = collision.GetComponent<PlayerController>();
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,player.GetComponent<Rigidbody2D>().velocity.y);
            player.canMove = false;
            Invoke("PlayerMovement", 0.5f);
       }
    }
    private void PlayerMovement()
    {
        player.canMove = true;
    }
}
