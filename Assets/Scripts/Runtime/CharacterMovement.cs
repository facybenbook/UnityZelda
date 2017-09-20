using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
	protected Rigidbody2D rbody;
	protected Animator anim;
	protected Vector2 lastMovement;
	public float speed;
	public Vector2 movement_vector;
	public Vector2 direction;
	// Use this for initialization
	void Start ()
	{
		lastMovement = Vector2.zero;
		direction = Vector2.down;
		rbody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		rbody.velocity = Vector3.zero;
	}
	// Update is called once per frame
	void Update ()
	{
		if (!GameController.control.gamePaused) {
			GetComponent<Animator> ().speed = 1;
			if (!GameController.control.gameOverState)
				Action ();
		}
	}

	protected virtual void Action ()
	{
	}

	public void Move (Vector2 direction, int multipleSpeed)
	{
		if (direction != Vector2.zero)
			rbody.MovePosition (rbody.position + direction *transform.lossyScale.x * speed * multipleSpeed * Time.deltaTime);
	}

	public void Jump (Vector2 direction, int multipleSpeed)
	{
		if (direction != Vector2.zero)
			rbody.MovePosition (rbody.position  + direction * multipleSpeed);
	}

}