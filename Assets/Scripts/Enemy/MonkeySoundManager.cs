using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeySoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip monkeyClip;
    public AudioClip jumpClip;

    public void MonkeySound()
    {
        audioSource.PlayOneShot(monkeyClip);
    }

    public void JumpMonkeySound()
    {
        audioSource.PlayOneShot(jumpClip);
    }
}
