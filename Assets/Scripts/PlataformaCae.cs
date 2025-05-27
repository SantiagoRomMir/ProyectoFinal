using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlataformaCae : MonoBehaviour
{

    public float tiempoEspera, tiempoReaparece;
    private Rigidbody2D rbd;
    private Vector3 posIni;

    // Start is called before the first frame update
    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        posIni = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("Caer", tiempoEspera);
            Invoke("Reaparece", tiempoReaparece);
        }
    }

    private void Caer()
    {
        rbd.constraints = RigidbodyConstraints2D.None;
        rbd.constraints = RigidbodyConstraints2D.FreezeRotation;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void Reaparece()
    {
        rbd.velocity = Vector3.zero;
        transform.position = posIni;
        rbd.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<BoxCollider2D>().enabled = true;
    }

}
