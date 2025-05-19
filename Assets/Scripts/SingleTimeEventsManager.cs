using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleTimeEventsManager : MonoBehaviour
{
    private List<int> events;
    // Este script puede controlar tambien eventos unicos de la escena como objetos recogidos o mecanismos activados
    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().clearPersistenceData)
        {
            events = new List<int>();
            Persistence.SavePersistenceVasesBroken(SceneManager.GetActiveScene().name, events);
        }
        else
        {
            LoadVasesBroken();

            DestroyVasesBroken();
        }

    }
    private void DestroyVasesBroken()
    {
        foreach (int i in events)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void GetBrokenVases()
    {
        events = new List<int>();
        for (int i=0; i<transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                events.Add(i);
            }
        }
    }
    public void AddBrokenVase(int index)
    {
        events.Add(index);
    }
    public void LoadVasesBroken()
    {
        events = Persistence.LoadPersistenceVaseBroken(SceneManager.GetActiveScene().name);
        DestroyVasesBroken();
    }
    public void SaveVasesBroken()
    {
        GetBrokenVases();
        string tmp = "";
        foreach (int i in events) tmp += i + " ";
        //Debug.Log(SceneManager.GetActiveScene().name + ":" + tmp);
        Persistence.SavePersistenceVasesBroken(SceneManager.GetActiveScene().name, events);
    }
}
