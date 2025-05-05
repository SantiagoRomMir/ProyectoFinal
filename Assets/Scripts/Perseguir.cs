using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perseguir : MonoBehaviour
{
    public GameObject enemy;
    private MeleeController meleeController;
    // Start is called before the first frame update
    void Start()
    {
        meleeController = enemy.GetComponent<MeleeController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            meleeController.chase = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            meleeController.chase = false;
            meleeController.ReturnPatrol();
        }
    }
}
