using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource music;
    private AudioSource sound;
    private void Awake()
    {
        music = GetComponents<AudioSource>()[0];
        sound = GetComponents<AudioSource>()[1];
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
