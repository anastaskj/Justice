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
    AudioClip[] mainMusicFiles;

    [SerializeField]
    AudioClip[] winMusicFiles;

    [SerializeField]
    AudioClip[] defeatMusicFiles;


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

    public void PlayDefeatMusic()
    {
        int rand = Random.Range(0, defeatMusicFiles.Length);
        source.clip = defeatMusicFiles[rand];
        source.Play();
    }

    public void PlayMainMenuMusic()
    {
        int rand = Random.Range(0, mainMusicFiles.Length);
        source.clip = mainMusicFiles[rand];
        source.Play();
    }
}
