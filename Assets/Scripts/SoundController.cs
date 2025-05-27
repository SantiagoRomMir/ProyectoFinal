using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource music;
    private AudioSource sound;

    [Header("Sound Effects")]
    public AudioClip[] breakVase;
    public AudioClip[] attacks;
    public AudioClip[] bows;
    public AudioClip[] crossbows;
    public AudioClip[] dodges;
    public AudioClip[] consumable;
    public AudioClip heal;
    public AudioClip hook;
    public AudioClip land;
    public AudioClip waterSplash;
    public AudioClip jump;
    public AudioClip shoot;
    public AudioClip playerLand;
    public AudioClip gunPowder;
    public AudioClip gunClick;
    public AudioClip aimHook;
    public AudioClip useHook;
    public AudioClip playerDeath;
    public AudioClip playerHurt;
    public AudioClip enemyHurt;
    public AudioClip[] enemyDeath;
    public AudioClip parry;
    public AudioClip parryPartial;
    public AudioClip[] footSteps;
    public AudioClip enemyAttack;
    private void Awake()
    {
        sound = GetComponents<AudioSource>()[0];
        music = GetComponents<AudioSource>()[1];
    }
    public void ChangeVolumeMusic(float volume)
    {
        music.volume = volume;
    }
    public void ChangeVolumeSound(float volume)
    {
        sound.volume = volume;
    }
    public AudioSource GetMusicSource()
    {
        return music;
    }
    public AudioSource GetSoundSource()
    {
        return sound;
    }
}
