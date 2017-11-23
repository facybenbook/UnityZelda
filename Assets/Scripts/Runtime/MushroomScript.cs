using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomScript : MonoBehaviour
{

    Transform head;
    Transform cap;
    Transform fillerBottom;
    Transform parent;
    public Sprite[] heads;

    // Use this for initialization
    void Start()
    {
        head = transform.Find("Head");
        cap = head.Find("Cap");
        fillerBottom = transform.Find("FillerBottom");
        parent = head.parent;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 headPosition = head.position - fillerBottom.position;
        //clamp stretching to 5
        if (Mathf.Abs(headPosition.y) > 5)
            head.transform.position = (headPosition.y > 0) ? new Vector3(head.transform.position.x, head.transform.position.y + 5f, 0) : new Vector3(head.transform.position.x, fillerBottom.position.y - 5f, 0);
        else if (Mathf.Abs(headPosition.x) > 5)
            head.transform.position = (headPosition.x > 0) ? new Vector3(fillerBottom.position.x + 5f, head.transform.position.y, 0) : new Vector3(fillerBottom.position.x - 5f, head.transform.position.y, 0);

        if (parent == head.parent)
        {
            head.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            //up
            if (headPosition.y > 0.5625f)
            {
                cap.GetComponent<SpriteRenderer>().sprite = heads[0];
                cap.GetComponent<SpriteRenderer>().sortingOrder = 3;

                head.transform.rotation = Quaternion.Euler(0, 0, 0);

                fillerBottom.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            //down
            else if (headPosition.y < -0.5625f)
            {
                cap.GetComponent<SpriteRenderer>().sprite = heads[2];
                cap.GetComponent<SpriteRenderer>().sortingOrder = 4;

                cap.rotation = Quaternion.Euler(0, 0, 180);

                fillerBottom.rotation = Quaternion.Euler(0, 0, 0);
            }
            //left
            else if (headPosition.x < -0.0625f)
            {
                cap.GetComponent<SpriteRenderer>().sprite = heads[1];
                cap.rotation = Quaternion.Euler(0, 0, 90);
                cap.localPosition = new Vector3(0, -3 * 0.0625f, 0);
                cap.GetComponent<SpriteRenderer>().flipX = true;

                fillerBottom.rotation = Quaternion.Euler(0, 0, -90);
            }
            //right
            else if (headPosition.x > 0.0625f)
            {
                cap.GetComponent<SpriteRenderer>().sprite = heads[1];
                cap.rotation = Quaternion.Euler(0, 0, -90);
                cap.localPosition = new Vector3(0, -3 * 0.0625f, 0);
                cap.GetComponent<SpriteRenderer>().flipX = false;

                fillerBottom.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                cap.GetComponent<SpriteRenderer>().sprite = heads[3];
                cap.GetComponent<SpriteRenderer>().sortingOrder = 4;
                cap.localPosition = new Vector3(0, 0, 0);
                cap.rotation = Quaternion.Euler(0, 0, 0);
                cap.GetComponent<SpriteRenderer>().flipX = false;

                head.position = new Vector3(fillerBottom.position.x, fillerBottom.position.y + 0.25f, 0);

                fillerBottom.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (Mathf.Abs(head.GetComponent<Rigidbody2D>().velocity.x) > 0 && Mathf.Abs(head.GetComponent<Rigidbody2D>().velocity.x) < 0.1f ||
                Mathf.Abs(head.GetComponent<Rigidbody2D>().velocity.y) > 0 && Mathf.Abs(head.GetComponent<Rigidbody2D>().velocity.y) < 0.1f)
            {
                head.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else
            head.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }
}
