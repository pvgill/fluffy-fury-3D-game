using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanVision : MonoBehaviour
{
    public GameObject happyCone;  
    public GameObject caughtCone; 

    private void Start()
    {
        happyCone.SetActive(true);
        caughtCone.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("player incoming vision: " + other.gameObject.name); 
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Player detected! Switching to red."); //debugg
            happyCone.SetActive(false);
            caughtCone.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left vision! Switching back to green."); 
            happyCone.SetActive(true);
            caughtCone.SetActive(false);
        }
    }
}
