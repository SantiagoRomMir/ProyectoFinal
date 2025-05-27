using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillsController : MonoBehaviour
{
    public enum PlayerSkills 
    { 
        Gun,
        Hook,
        Parrot
    }
    public PlayerSkills skill;
    public bool hideSprite;
    private void Awake()
    {
        if (hideSprite)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision!=null && collision.CompareTag("Player"))
        {
            switch (skill)
            {
                case PlayerSkills.Gun:
                    collision.GetComponent<PlayerController>().hasGun = true;
                    collision.GetComponent<PlayerController>().hudControl.ActiveGunPowder();
                    PlayerPrefs.SetString("hasGun", "true");
                    break;
                case PlayerSkills.Hook:
                    collision.GetComponent<PlayerController>().hasHook = true;
                    PlayerPrefs.SetString("hasHook", "true");
                    break;
                case PlayerSkills.Parrot:
                    collision.GetComponent<PlayerController>().hasParrot = true;
                    PlayerPrefs.SetString("hasParrot", "true");
                    break;
            }
            Destroy(gameObject);
        }
    }
}
