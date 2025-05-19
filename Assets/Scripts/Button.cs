using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject Texto;
    // Start is called before the first frame update
    public void OnClickJugar()
    {

    }
    public void OnClick()
    {
        Texto.SetActive(false);
        GetComponent<Animator>().Play("boton");
    }

}
