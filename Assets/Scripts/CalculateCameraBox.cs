using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateCamera : MonoBehaviour
{
    private Camera cam;
    private BoxCollider2D camBox;
    private float sizeX, sizeY, ratio;
    void Start()
    {
        cam = GetComponent<Camera>();
        camBox = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        sizeY = cam.orthographicSize * 2;
        ratio = (float)Screen.width / (float)Screen.height;
        sizeX = sizeY * ratio;
        camBox.size = new Vector2 (sizeX, sizeY);
    }
}
