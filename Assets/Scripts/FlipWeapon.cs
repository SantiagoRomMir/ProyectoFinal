using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipWeapon : MonoBehaviour
{
    public Transform transformL, transformR;
    private void Update()
    {
        FlipPosition();
    }
    public void FlipPosition()
    {
        if (transform.parent.GetComponent<SpriteRenderer>().flipX)
        {
            transform.position = transformL.transform.position;
        }
        else
        {
            transform.position = transformR.transform.position;
        }
    }
}
