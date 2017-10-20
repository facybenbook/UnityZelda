using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomScript : MonoBehaviour {

    Transform head;
    Transform cap;
    Transform fillerBottom;
    public Sprite[] heads;

	// Use this for initialization
	void Start () {
        head = transform.Find("Mushroom_Cap");
        cap = head.Find("Cap");
        fillerBottom = transform.Find("FillerBottom");
	}

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(head.transform.localPosition.y) > 5)
            head.transform.localPosition = (head.transform.localPosition.y > 0) ? new Vector3(head.transform.localPosition.x, 5, 0) : new Vector3(head.transform.localPosition.x, -5, 0);
        else if (Mathf.Abs(head.transform.localPosition.x) > 5)
            head.transform.localPosition = (head.transform.localPosition.x > 0) ? new Vector3(5, head.transform.localPosition.y, 0) : new Vector3(-5, head.transform.localPosition.y, 0);
        //up
        if (head.transform.localPosition.y > 0.5625f)
            {
                cap.GetComponent<SpriteRenderer>().sprite = heads[0];
                cap.GetComponent<SpriteRenderer>().sortingOrder = 3;

                head.transform.rotation = Quaternion.Euler(0, 0, 0);

                fillerBottom.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            //down
            else if (head.transform.localPosition.y < -0.5625f)
            {
                cap.GetComponent<SpriteRenderer>().sprite = heads[2];
                cap.GetComponent<SpriteRenderer>().sortingOrder = 4;

                cap.transform.rotation = Quaternion.Euler(0, 0, 180);

                fillerBottom.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            //left
            else if (head.transform.localPosition.x < -0.0625f)
            {
                cap.GetComponent<SpriteRenderer>().sprite = heads[1];
                cap.transform.rotation = Quaternion.Euler(0, 0, 90);
                cap.transform.localPosition = new Vector3(0, -3 * 0.0625f, 0);
                cap.GetComponent<SpriteRenderer>().flipX = true;

                fillerBottom.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            //right
            else if (head.transform.localPosition.x > 0.0625f)
            {
                cap.GetComponent<SpriteRenderer>().sprite = heads[1];
                cap.transform.rotation = Quaternion.Euler(0, 0, -90);
                cap.transform.localPosition = new Vector3(0, -3 * 0.0625f, 0);
                cap.GetComponent<SpriteRenderer>().flipX = false;

                fillerBottom.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                cap.GetComponent<SpriteRenderer>().sprite = heads[3];
                cap.GetComponent<SpriteRenderer>().sortingOrder = 4;
                cap.transform.localPosition = new Vector3(0, 0, 0);
                cap.transform.rotation = Quaternion.Euler(0, 0, 0);
                cap.GetComponent<SpriteRenderer>().flipX = false;

                head.transform.localPosition = new Vector3(0, 0.5f, 0);

                fillerBottom.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        if (Mathf.Abs(head.GetComponent<Rigidbody2D>().velocity.x) > 0 && Mathf.Abs(head.GetComponent<Rigidbody2D>().velocity.x) < 0.1f ||
            Mathf.Abs(head.GetComponent<Rigidbody2D>().velocity.y) > 0 && Mathf.Abs(head.GetComponent<Rigidbody2D>().velocity.y) < 0.1f)
        {
            head.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
