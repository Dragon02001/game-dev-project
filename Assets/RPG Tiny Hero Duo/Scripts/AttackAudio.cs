using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAudio : MonoBehaviour
{
    public AudioClip attackSound;

    private AudioSource audioSource;

    void start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackSound);
    }
}
