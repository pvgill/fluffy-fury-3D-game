using UnityEngine;

public class DoorTriggerActivator : MonoBehaviour
{
    public Door doorToCheck; // Assign the door you want to check
    
    void Start()
    {
        // Disable the trigger initially
        GetComponent<BoxCollider>().enabled = false;
    }
    
    void Update()
    {
        if (doorToCheck != null)
        {
            // Enable the trigger only when the door is not active (locked)
            GetComponent<BoxCollider>().enabled = !doorToCheck.isActive;
        }
    }
}