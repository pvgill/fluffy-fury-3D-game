//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerCloseby : MonoBehaviour
//{
//    private Animator anim;
//    public bool playerEnter = false;

//    void Awake()
//    {
//        anim = GetComponent<Animator>();

//        if (anim == null)
//        {
//            Debug.LogError("Animator not found");
//        }
//    }
//    void OnTriggerEnter(Collider c)
//    {
//        if (c.attachedRigidbody != null)
//        {
//            BallCollector bc = c.attachedRigidbody.GetComponent<BallCollector>();
//            playerEnter = true;

//            if (bc != null)
//            {
//                Debug.Log("player enter");
//                anim.SetBool("playerClose", true);
//            }
//        }
//    }
//    void OnTriggerExit(Collider c)
//    {
//        if (c.attachedRigidbody != null)
//        {
//            BallCollector bc = c.attachedRigidbody.GetComponent<BallCollector>();
//            playerEnter = false;

//            if (bc != null)
//            {
//                Debug.Log("player exit");
//                anim.SetBool("playerClose", false);
//            }
//        }
//    }
//}
