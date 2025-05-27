using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalancaPuertaFinal : MonoBehaviour
{
    private SpriteRenderer rendererDeImagen;
    public Sprite nuevaImagen;
    // Start is called before the first frame update
    void Awake()
    {
        rendererDeImagen = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AbrirPuertaFinal();
        }
    }
    void AbrirPuertaFinal()
    {
        PlayerPrefs.SetInt("puertafinal", 1);
        rendererDeImagen.sprite = nuevaImagen;
        Destroy(this);
    }
}
