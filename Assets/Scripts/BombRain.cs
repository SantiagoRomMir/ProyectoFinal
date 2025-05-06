using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombRain : MonoBehaviour
{
    public int minDelay, maxDelay, bombers;
    public GameObject bomb;
    private void Start()
    {
        Debug.Log("Bombs Dropping in Range: " + (transform.position.x - transform.localScale.x / 2) + " - " + (transform.localScale.x + transform.localScale.x / 2));

        StartCoroutine("StartBombing", bombers);
    }
    IEnumerator DropBomb()
    {
        while (true)
        {
            Vector2 startPos = new Vector2(Random.Range(transform.position.x-transform.localScale.x/2, transform.position.x + transform.localScale.x / 2), transform.position.y);
            GameObject b = Instantiate(bomb);
            b.transform.position = startPos;
            b.transform.parent = transform;
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay+1));
        }
    }
    IEnumerator StartBombing(int times)
    {
        for (int i=0; i<times; i++)
        {
            StartCoroutine("DropBomb");
            yield return new WaitForSeconds(Random.Range(1,4));
        }
    }
}
