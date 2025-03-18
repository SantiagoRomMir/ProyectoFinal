using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coleccionable : MonoBehaviour
{
    public enum TipePowerUp
    {
        Datil,
        Ron,
    }
    public TipePowerUp powerUp;
    public int numRon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            switch (powerUp)
            {
                case TipePowerUp.Ron:
                    collision.GetComponent<PlayerController>().ReplenishRon(numRon);
                    Destroy(gameObject);
                    break;
                case TipePowerUp.Datil:
                    Destroy(gameObject);
                    break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
