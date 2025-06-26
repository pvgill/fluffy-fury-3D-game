using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip barkClip;

    public void BarkSound()
    {
        audioSource.PlayOneShot(barkClip);
    }
}
