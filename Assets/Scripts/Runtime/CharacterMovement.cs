using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
	protected Rigidbody2D rbody;
	protected Animator anim;
	protected Vector2 lastMovement;
	public int pixelPerFrameSpeed;
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

	void LaterUpdate()
	{
		Vector3 tmp;
		tmp.x = (int)(this.transform.position.x / 0.0625f) * 0.0625f;
		tmp.y = (int)(this.transform.position.y / 0.0625f) * 0.0625f;
		tmp.z = 0;
		this.transform.position = tmp;
	}

	protected virtual void Action ()
	{
	}

	public void Move (Vector2 direction, int multipleSpeed)
	{
		if (direction != Vector2.zero)
			//increment position considering the pixel size, the scale of the object and the number of pixels per frame of deplacement
			rbody.MovePosition (rbody.position + direction * (0.625f * transform.lossyScale.x * pixelPerFrameSpeed * Time.deltaTime));
	}

	public void Jump (Vector2 direction, int multipleSpeed)
	{
		if (direction != Vector2.zero)
			rbody.MovePosition (rbody.position  + direction * multipleSpeed);
	}

}