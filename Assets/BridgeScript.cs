using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeScript : MonoBehaviour {

    public GameObject over;
    public GameObject under;

    void SwitchBridge(GameObject bridgeSide)
    {
        if (bridgeSide == over)
        {
            Debug.Log("Over");
            foreach (Transform child in over.transform)
            {
                child.gameObject.SetActive(true);
            }
            foreach (Transform child in under.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else if (bridgeSide == under)
        {
            Debug.Log("Under");
            foreach (Transform child in over.transform)
            {
                child.gameObject.SetActive(false);
            }
            foreach (Transform child in under.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
