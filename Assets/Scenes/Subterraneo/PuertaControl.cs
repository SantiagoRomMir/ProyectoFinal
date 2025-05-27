using UnityEngine;

public class PuertaControl : MonoBehaviour {
    
    public GameObject[] puerta;
    private SpriteRenderer rendererDeImagen;
    public Sprite nuevaImagen;
    void Awake()
    {
        rendererDeImagen = GetComponent<SpriteRenderer>();
        if (PlayerPrefs.GetInt("Palanca") == 1)
        {
            AbrirPuerta();
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            AbrirPuerta();
        }
    }

    public void AbrirPuerta()
    {
        PlayerPrefs.SetInt("Palanca", 1);
        for (int i = 0; i < puerta.Length; i++)
        {
            puerta[i].SetActive(false);
        }
        rendererDeImagen.sprite = nuevaImagen;
        Destroy(this);
    }
}