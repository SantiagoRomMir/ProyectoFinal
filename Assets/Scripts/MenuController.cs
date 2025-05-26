using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuMusic;
    public AudioClip musicClip;
    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("MenuMusic")==null && SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            GameObject music = Instantiate(menuMusic);
            music.tag = "MenuMusic";
            music.GetComponent<SoundController>().GetMusicSource().PlayOneShot(musicClip);
            DontDestroyOnLoad(music);
        }
    }
    private IEnumerator Play()
    {
        GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<SoundController>().GetSoundSource().PlayOneShot(GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<SoundController>().enemyAttack);
        yield return new WaitForSeconds(0.75f);
        Destroy(GameObject.FindGameObjectWithTag("MenuMusic"));
        PlayerPrefs.SetInt("clearPersistenceData", 1);
        SceneManager.LoadScene("Inicio");
    }
    public void Jugar()
    {
        StartCoroutine("Play");
    }
    public IEnumerator Controls()
    {
        GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<SoundController>().GetSoundSource().PlayOneShot(GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<SoundController>().enemyAttack);
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene("Controls");
    }
    public void Controles()
    {
        StartCoroutine("Controls");
    }
    public IEnumerator Credits()
    {
        GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<SoundController>().GetSoundSource().PlayOneShot(GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<SoundController>().enemyAttack);
        yield return new WaitForSeconds(0.75f);
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
        GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<SoundController>().GetSoundSource().PlayOneShot(GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<SoundController>().enemyAttack);
        yield return new WaitForSeconds(0.75f);
        Application.Quit();
    }
    public void Menu()
    {
        StartCoroutine("MainMenu");
    }
    private IEnumerator MainMenu()
    {
        GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<SoundController>().GetSoundSource().PlayOneShot(GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<SoundController>().enemyAttack);
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene("MainMenu");
    }
}
