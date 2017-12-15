using UnityEngine;
using System.Collections;

public class LifeController : MonoBehaviour {
	public int maxHealth = 12;
	public int health;
	public bool escape = true;
	public AudioClip hurtSound;
	public AudioClip deathSound;
	private AudioSource audioSource;
	public bool dead;
	// Use this for initialization
	void Start ()
	{
		dead = false;
        health = maxHealth;
		audioSource = gameObject.AddComponent<AudioSource>();
		health = maxHealth;
	}
	public void Hurt(int damage, Vector3 positionToEscape)
	{
		if (damage > 0)
		{
			health -= damage;
			if (health < 0)
				health = 0;
			//movement to escape hit && lock animator
			GetComponent<Animator> ().SetBool ("is_busy", true);
			GetComponent<Animator> ().SetTrigger("is_hurt");
			//sets the objects movement vector to escape
			if (escape == true) {
                GetComponent<ActorController> ().Escape(positionToEscape, 4);
			}
			else
			{
				GetComponent<ActorController> ().movementDirection = Vector2.zero;
			}
			audioSource.PlayOneShot (hurtSound);
			if (health == 0) {
				GetComponent<Animator> ().SetBool ("is_dead", true);
				audioSource.PlayOneShot (deathSound);
				dead = true;
			}
		}
		if (gameObject.name == "Player")
			GameController.control.playerStats.health = health;
	}
}
