using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampaVertical : MonoBehaviour
{
    public GameObject rocks;
    public GameObject player;
    public SpriteRenderer ObjectSprite;
    public Sprite sprite;
    public Sprite rocksSprite;
    public Sprite ArrowSprite;
    public PlayerController control;
    public bool verticalTrap;
    public bool HorizontalTrapRight;
    // Start is called before the first frame update
    void Update()
    {
        if (verticalTrap == true)
        {
            sprite = rocksSprite;
            transform.Translate(Vector2.down * 10 * Time.deltaTime);
        }
        else if (HorizontalTrapRight == true)
        {
            sprite = ArrowSprite;
            transform.Translate(Vector2.right * 3 * Time.deltaTime);
        }
        else
        {
            sprite = ArrowSprite;
            ObjectSprite.flipX = true;
            transform.Translate(Vector2.left * 3 * Time.deltaTime);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            control.HurtPlayer(5, transform.position, true, false);
        }
    }
}
