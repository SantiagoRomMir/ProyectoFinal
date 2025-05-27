using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float StartPos;
    public GameObject cam;
    public float parallax;

    private void Start()
    {
        StartPos = transform.position.x;
    }

    private void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallax;

        transform.position = new Vector3(StartPos + distance, transform.position.y, transform.position.z);
    }
}
