//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AI_Vision : MonoBehaviour {
//    public enum VisionType {radius, line, angle}
//    public VisionType visionType;
//    public float range = 0;

//    protected Collider2D visionCollider;
//    void Start()
//    {
//        if (range == 0)
//            range = 20f;
//        switch (visionType)
//        {
//            case VisionType.radius:
//                { 
//                CircleCollider2D tmp = gameObject.AddComponent<CircleCollider2D>();
//                tmp.radius = range;
//                visionCollider = tmp;
//                break;
//                }
//            case VisionType.line:
//                {
//                    EdgeCollider2D tmp = gameObject.AddComponent<EdgeCollider2D>();
//                    visionCollider = tmp;
//                    break;
//                }
//        }
//        visionCollider.isTrigger = true;
//    }
//    //if the monster sees the player (visual radius), appears
//    void OnTriggerEnter2D(Collider2D coll)
//    {
//        if (coll.gameObject.tag == "Player" && hidden == true)
//        {
//            GetComponent<Animator>().SetBool("is_coming", true);
//            GetComponent<CircleCollider2D>().radius = 6;
//            hidden = false;
//        }
//    }

//    //while the player is in the visual radius of the monster, chase the player
//    void OnTriggerStay2D(Collider2D coll)
//    {
//        //verify if the object in the radius is the player
//        if (coll.gameObject.tag == "Player")
//        {
//            //move toward the player
//            movementDirection.x = (coll.gameObject.transform.position.x - rbody.position.x);
//            movementDirection.y = coll.gameObject.transform.position.y - rbody.position.y;
//            movementDirection.Normalize();
//        }
//    }
//    void OnTriggerExit2D(Collider2D coll)
//    {
//        if (coll.gameObject.tag == "Player")
//        {
//            //when the player leaves the visual zone
//            movementDirection = Vector2.zero;
//        }
//    }
//}
