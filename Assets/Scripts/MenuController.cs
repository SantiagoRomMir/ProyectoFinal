using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private IEnumerator Play()
    {
        yield return new WaitForSeconds(1f);
        PlayerPrefs.SetInt("clearPersistenceData", 1);
        SceneManager.LoadScene("Inicio");
    }
    public void Jugar()
    {
        StartCoroutine("Play");
    }
    public IEnumerator Controls()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Controls");
    }
    public void Controles()
    {
        StartCoroutine("Controls");
    }
    public IEnumerator Credits()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Credits");
    }
    public void Creditos()
    {
        StartCoroutine("Credits");
    }
    public void Salir()
    {
        StartCoroutine("exit");
    }
    private IEnumerator exit()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
    public void Menu()
    {
        StartCoroutine("MainMenu");
    }
    private IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainMenu");
    }
}
