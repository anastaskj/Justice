using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource source;

    [SerializeField]
    AudioClip[] unitMove;

    [SerializeField]
    AudioClip[] unitAttack;

    [SerializeField]
    AudioClip[] unitDeath;

    [SerializeField]
    AudioClip[] unitAbility;

    public void MoveSound()
    {
        int rand = Random.Range(0, unitMove.Length);
        source.PlayOneShot(unitMove[rand]);
    }

    public void AttackSound()
    {
        int rand = Random.Range(0, unitAttack.Length);
        source.PlayOneShot(unitAttack[rand]);
    }

    public void DeathSound()
    {
        int rand = Random.Range(0, unitDeath.Length);
        source.PlayOneShot(unitDeath[rand]);
    }

    public void AbilitySound()
    {
        int rand = Random.Range(0, unitAbility.Length);
        source.PlayOneShot(unitAbility[rand]);
    }
}
