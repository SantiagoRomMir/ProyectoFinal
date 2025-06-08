using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject Texto;
    public void OnClick()
    {
        Texto.SetActive(false);
        GetComponent<Animator>().Play("boton");
    }

}
