using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioSource source;

    [SerializeField]
    AudioClip[] musicFiles;

    [SerializeField]
    AudioClip[] winMusicFiles;


    public void PlayMusic()
    {
        int rand = Random.Range(0, musicFiles.Length);
        source.clip = musicFiles[rand];
        source.Play();
    }

    public void PlayWinMusic()
    {
        int rand = Random.Range(0, winMusicFiles.Length);
        source.clip = winMusicFiles[rand];
        source.Play();
    }
}
