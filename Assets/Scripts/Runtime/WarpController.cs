using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpController : MonoBehaviour {
	
	public Object scene;
	public Vector2 destination;

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.tag == "Player")
		{
			if (SceneManager.GetActiveScene().name != scene.name && scene != null) 
			{
				SceneManager.LoadScene (scene.name);
			}

			coll.transform.position = new Vector3 (destination.x, destination.y, 0);
		}
	}
}
