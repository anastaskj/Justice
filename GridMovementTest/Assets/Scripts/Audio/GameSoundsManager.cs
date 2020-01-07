using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundsManager : MonoBehaviour
{
    [SerializeField]
    AudioSource source;

    [SerializeField]
    AudioClip[] buttonClick;

    public void ButtonClickSound()
    {
        int rand = Random.Range(0, buttonClick.Length);
        source.PlayOneShot(buttonClick[rand]);
    }
}
