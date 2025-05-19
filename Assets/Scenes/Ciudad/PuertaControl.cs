using UnityEngine;

public class PuertaControl : MonoBehaviour {
    
    public GameObject puerta;
    public SpriteRenderer rendererDeImagen;
    public Sprite nuevaImagen;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            AbrirPuerta();
            rendererDeImagen.sprite = nuevaImagen; 
        }
    }

    public void AbrirPuerta() {
        puerta.SetActive(false);
    }
}