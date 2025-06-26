using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapEvent : MonoBehaviour
{
    private int count = 0;
    private bool raised = false;

    Animator trap_Animator;

    private void Start()
    {
        if (this.gameObject.tag == "trap")
        {
            trap_Animator = gameObject.GetComponent<Animator>();
        } else
        {
            Debug.LogError("not a trap");
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            SpikeActivator sa = c.attachedRigidbody.gameObject.GetComponent<SpikeActivator>();
            if (sa != null)
            {
                count += 1;
                if (!raised)
                {
                    raised = true;
                    trap_Animator.SetTrigger("raiseTrap");
                    trap_Animator.ResetTrigger("lowerTrap");
                }
            }

        }
    }
    private void OnTriggerExit(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            SpikeActivator sa = c.attachedRigidbody.gameObject.GetComponent<SpikeActivator>();
            if (sa != null)
            {
                count -= 1;
                if (count == 0)
                {
                    raised = false;
                    trap_Animator.SetTrigger("lowerTrap");
                    trap_Animator.ResetTrigger("raiseTrap");
                }
            }

        }
    }
}
