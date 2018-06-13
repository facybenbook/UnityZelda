using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSender : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            transform.parent.SendMessage("SwitchBridge", gameObject);
        }
    }
}
