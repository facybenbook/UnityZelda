using UnityEngine;
using System.Collections;

public class CharactersController : MonoBehaviour
{
	public int pixelPerFrameSpeed;
	public Vector2 movementDirection;
    public Vector2 characterOrientation;
    public bool dead;
    protected Collider2D hitbox;
    protected LifeController lifeController;
    protected Rigidbody2D rbody;
    public Animator anim;

    protected virtual void Start ()
	{
        dead = false;
        lifeController = GetComponent<LifeController>();
        characterOrientation = Vector2.down;
		movementDirection = Vector2.down;
		rbody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		rbody.velocity = Vector3.zero;
	}

	void FixedUpdate ()
	{
		if (!GameController.control.gamePaused) {
			GetComponent<Animator> ().speed = 1;
			if (!GameController.control.gameOverState)
				Action ();
		}
	}

    /// <summary>
    /// Actions to perform each frame: AI for monsters, managing inputs for player
    /// </summary>
    protected virtual void Action ()
	{
        if (anim.GetBool("is_hurt") == false && lifeController.dead == false)
        {
            rbody.velocity = Vector3.zero;
            Move(1);
        }
    }
    /// <summary>
    /// Move the gameObject along its movement vector
    /// </summary>
    /// <param name="speedMult"></param>
	public void Move (int speedMult)
	{
		if (movementDirection != Vector2.zero)
			//increment position considering the pixel size, the scale of the object and the number of pixels per frame of deplacement
			rbody.MovePosition (rbody.position + movementDirection * (0.625f * transform.lossyScale.x * pixelPerFrameSpeed * speedMult * Time.deltaTime));
	}

    /// <summary>
    /// WIP
    /// make the gameObject jump in a direction with a speed
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
	public void Jump (Vector2 direction, int speed)
	{
		if (direction != Vector2.zero)
        {
            rbody.MovePosition (rbody.position + movementDirection * (0.625f * transform.lossyScale.x * 16));
        }
    }

    /// <summary>
    /// Actions to perform when the GameController pauses the game
    /// </summary>
    protected virtual void OnPause()
    {
    }

    protected virtual void OnResume()
    {
    }
}