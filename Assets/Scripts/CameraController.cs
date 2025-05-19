using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private float posZ;
    private bool isFollowing;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        posZ = transform.position.z;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraMovement();
    }
    private void CameraMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y + 2, posZ),Time.deltaTime*player.GetComponent<PlayerController>().speed*0.75f);
    }
}
