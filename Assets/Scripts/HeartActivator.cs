using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartActivator : MonoBehaviour
{
    public bool nearHeart = false;

    public void nearHeartCollider()
    {
        nearHeart = true;
    }
}