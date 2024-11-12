using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource musicSource;
    [Header("Sonidos")]
    [SerializeField] private AudioClip readySound;
    [SerializeField] private AudioClip startSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [Header("Música")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;
    
    void Start()
    {
        musicSource.clip = menuMusic;
        musicSource.Play();
    }

    public void PlayReadySound()
    {
        audioSource.PlayOneShot(readySound);
    }

    public void PlayStartSound()
    {
        audioSource.PlayOneShot(startSound);
    }

    public void PlayGameMusic()
    {
        musicSource.clip = gameMusic;
        musicSource.Play();
    }

    public void PlayWinSound()
    {
        audioSource.PlayOneShot(winSound);
    }

    public void PlayLoseSound()
    {
        audioSource.PlayOneShot(loseSound);
    }
}
