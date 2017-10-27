using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stretchBetween : MonoBehaviour {

    public GameObject a;
    public GameObject b;
    public float ratio;
	
	// Update is called once per frame
	void Update () {
        Vector3 tmp = a.transform.position - b.transform.position;
        float value;
        if (Mathf.Abs(tmp.x) > Mathf.Abs(tmp.y))
            value = tmp.x;
        else
            value = tmp.y;
        transform.localScale = new Vector3(1, Mathf.Abs(value) * 8 * ratio, 1);
    }
}
