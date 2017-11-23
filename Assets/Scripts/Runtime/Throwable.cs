using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour {
    public bool throwed;
    public bool hit = false;
    private Rigidbody2D body;
    private float speed = 4f;
    private Vector3 movementVector;
    //public IEnumerator Lift(Vector2 destination)
    //{

    //}

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (throwed)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + movementVector.z * speed * Time.fixedDeltaTime);
            body.MovePosition(body.position + new Vector2(movementVector.x, movementVector.y) * speed * Time.fixedDeltaTime);
        }
    }

    public void Throw(Vector2 direction)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f);
        movementVector = Vector3.zero;

        if (direction == Vector2.zero)
            movementVector.y = -0.5f;
        else
        {
            movementVector = direction * 6;
            if (movementVector.y == 0)
                movementVector.y = -0.5f;
        }
        movementVector.z = -0.5f;
        throwed = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (throwed)
        {
            if (collision.isTrigger == false && collision.gameObject.tag != "Player")
            {
                if (transform.position.z <= 0.5f)
                {
                    hit = true;
                    movementVector = Vector3.zero;
                    GetComponent<Collider2D>().enabled = false;
                    GetComponent<Destructible>().DestructionPhase();
                }
            }
        }
    }
}
