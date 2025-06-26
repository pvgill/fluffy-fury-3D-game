using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSpinTrigger : MonoBehaviour
{
    private Animator heartAnimator;

    void Start()
    {
        heartAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            HeartActivator ha = c.attachedRigidbody.gameObject.GetComponent<HeartActivator>();
            if (ha != null)
            {
                heartAnimator.SetBool("isSpinning", true);
                Debug.Log("Player is close: Heart starts spinning.");
            }
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            HeartActivator ha = c.attachedRigidbody.gameObject.GetComponent<HeartActivator>();
            if (ha != null)
            {
                heartAnimator.SetBool("isSpinning", false);
                Debug.Log("Player is far: Heart resets to idle.");
            }
        }
    }
}