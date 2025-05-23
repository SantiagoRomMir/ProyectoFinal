using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseController : MonoBehaviour
{
    [Header("MoneyDrop")]
    public GameObject money;
    public float moneyMultiplier;
    private SoundController soundController;
    private void Start()
    {
        soundController = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>();
    }
    public void DropMoney()
    {
        PlayBreakSound();
        for (int i = 0; i < Random.Range(1, 6) + 1 * moneyMultiplier; i++)
        {
            int randMoney = (int)(Random.Range(5, 16) + 1 * moneyMultiplier);
            money.GetComponent<ConsumableController>().money = randMoney;
            money.transform.position = transform.position;
            Instantiate(money);
        }
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
    private void PlayBreakSound()
    {
        AudioClip sound = soundController.breakVase[Random.Range(0,soundController.breakVase.Length-1)];
        soundController.GetSoundSource().PlayOneShot(sound);
    }
}
