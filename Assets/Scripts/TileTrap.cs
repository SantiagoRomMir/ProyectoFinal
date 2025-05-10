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

            // Posicion Segura alejada de la trampa -> Si el jugador cae de la izquierda lo moverá -1 y viceversa
            float safePosModifier = collision.GetComponent<PlayerController>().movementDirAbs;

            Vector3 savePos = collision.gameObject.GetComponent<PlayerController>().lastPosition;

            // Posicion final del jugador una vez sido alejado de la trampa y elevado del suelo ligeramente
            Vector3 newPlayerPos = new Vector3(savePos.x + -safePosModifier, savePos.y+2);
            // Calcula la altura a la que debe colocar al jugador para evitar que atraviese el suelo
            RaycastHit2D hit;
            while (!(hit = Physics2D.Raycast(newPlayerPos, Vector3.up)).collider.isTrigger) 
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
