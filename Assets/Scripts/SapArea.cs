using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapArea : MonoBehaviour
{
    public float slowFactor = 0.1f;

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("Trigger entered by: " + collision.name);

        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    DogAI movement = collision.GetComponent<DogAI>();
        //    if (movement != null)
        //    {
        //        Debug.Log("Enter sap");
        //        movement.EnterSapArea(slowFactor);
        //    }
        //    else
        //    {
        //        Debug.Log("DogAI not found");
        //    }
        //}
    }

    private void OnTriggerExit(Collider collision)
    {
        //Debug.Log("Trigger exited by: " + collision.name);

        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    DogAI movement = collision.GetComponent<DogAI>();
        //    if (movement != null)
        //    {
        //        Debug.Log("Exit sap");
        //        movement.ExitSapArea(slowFactor);
        //    }
        //    else
        //    {
        //        Debug.Log("DogAI not found");
        //    }

        //}
    }
}
