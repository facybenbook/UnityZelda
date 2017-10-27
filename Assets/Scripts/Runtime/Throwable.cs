using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour {
    public bool throwed;
    public bool hit = false;
    

    //public IEnumerator Lift(Vector2 destination)
    //{

    //}

    public IEnumerator Propulse(Vector2 orientation)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float speed = 4;
        Vector3 position = transform.localPosition;
        float z = 0.5f;
        Vector2 fallingVector = Vector2.zero;

        if (orientation == Vector2.zero)
            fallingVector.y = -z;
        else
        {
            fallingVector = orientation * 6;
            if (fallingVector.y == 0)
                fallingVector.y = -z;
        }

        
        while (z > 0)
        {
            if (hit)
                break;
            rb.MovePosition(rb.position + (fallingVector * speed * Time.deltaTime));
            yield return null;
            z -= speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.gameObject.tag != "Player")
        {
            hit = true;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GetComponent<Destructible>().DestructionPhase();
            //Destroy(gameObject);
        }
    }
}
