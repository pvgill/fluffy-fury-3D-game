//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class PlayerEnterCage : MonoBehaviour
//{
//    public bool playerEnter = false;

//    void OnTriggerEnter(Collider c)
//    {
//        if (c.attachedRigidbody != null)
//        {
//            BallCollector bc = c.attachedRigidbody.GetComponent<BallCollector>();

//            if (bc != null)
//            {
//                playerEnter = true;

//                CageMovement cageMovement = transform.parent.GetComponent<CageMovement>();

//                if (cageMovement != null)
//                {
//                    cageMovement.PlayerEnter();
//                }
//            }
//        }
//    }

//    void OnTriggerExit(Collider c)
//    {
//        if (c.attachedRigidbody != null)
//        {
//            BallCollector bc = c.attachedRigidbody.GetComponent<BallCollector>();

//            if (bc != null)
//            {
//                playerEnter = false;

//                CageMovement cageMovement = transform.parent.GetComponent<CageMovement>();

//                if (cageMovement != null)
//                {
//                    cageMovement.PlayerLeave();
//                }
//            }
//        }
//    }
//}
