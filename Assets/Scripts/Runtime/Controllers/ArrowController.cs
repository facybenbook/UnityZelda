using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    bool stop;
    public float speed;
    public int damages;
    public Vector2 direction;
    private Rigidbody2D rbody;
    // Use this for initialization
    void Start()
    {
        stop = false;
        rbody = GetComponent<Rigidbody2D>();
        if (transform.rotation.eulerAngles.z == 0)
            direction = Vector2.up;
        else if (transform.rotation.eulerAngles.z == 90)
            direction = Vector2.left;
        else if (transform.rotation.eulerAngles.z == 180)
            direction = Vector2.down;
        else if (transform.rotation.eulerAngles.z == 270)
            direction = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        if (stop == false)
        {
           rbody.MovePosition(rbody.position + direction * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<LifeController>())
        {
            coll.gameObject.GetComponent<LifeController>().Hurt(damages, transform.position);
            Destroy(this.gameObject);
        }
        else if (coll.isTrigger == false)
        {
            stop = true;
            GetComponent<Animator>().SetTrigger("stuck");
        }
    }
}
