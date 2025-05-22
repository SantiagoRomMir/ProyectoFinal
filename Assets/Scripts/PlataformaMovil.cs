using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovil : MonoBehaviour {
    
    public GameObject ObjetoaMover;
    public Transform StartPoint, EndPoint;
    public float Velocidad;
    public Vector3 MoverHacia;

    // Start is called before the first frame update
    void Start() {
        MoverHacia = EndPoint.position;
    }

    // Update is called once per frame
    void Update() {
        ObjetoaMover.transform.position = Vector3.MoveTowards(ObjetoaMover.transform.position, MoverHacia, Velocidad * Time.deltaTime);



        if(ObjetoaMover.transform.position == EndPoint.position) {
            MoverHacia = StartPoint.position;

        } else if (ObjetoaMover.transform.position == StartPoint.position) {
            MoverHacia = EndPoint.position;
        }
    }
}
