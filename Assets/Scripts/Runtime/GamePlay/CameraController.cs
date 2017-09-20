using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public GameObject target;
	public GameObject mapTarget;
	public bool transitionToNewTarget;
	public int speed;

	private float minX;
	private float minY;
	private float maxX;
	private float maxY;
	// Use this for initialization
	void Start ()
	{
		//boundaries from left top corner
		minX = mapTarget.transform.position.x;
		maxX = minX + mapTarget.GetComponent<Tiled2Unity.TiledMap> ().NumTilesWide;
		minY = mapTarget.transform.position.y;
		maxY = minY - mapTarget.GetComponent<Tiled2Unity.TiledMap> ().NumTilesHigh;
		speed = 6;
		if (target == null)
			target = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
		Move ();
	}

	void ChangeTarget (GameObject newTarget, bool transition)
	{
		if (newTarget != null) 
		{
			target = newTarget;
			transitionToNewTarget = transition;
		}
	}
	void ChangeMap (GameObject newMap)
	{
		if (newMap != null) 
		{
			minX = mapTarget.transform.position.x;
			maxX = mapTarget.GetComponent<Tiled2Unity.TiledMap> ().NumTilesWide + minX;
			minY = mapTarget.transform.position.y;
			maxY = mapTarget.GetComponent<Tiled2Unity.TiledMap> ().NumTilesHigh + minY;
			mapTarget = newMap;
			//transitionToNewMap = transition;
		}
		else
			minX = 0;
			maxX = 15;
			minY = 0;
			maxY = 10;
	}

	void Move ()
	{
		if (target != null)
		{
			if (transitionToNewTarget)
			{
				this.transform.position = Vector3.MoveTowards (this.transform.position, target.transform.position, speed * Time.deltaTime);
				if (this.transform.position == target.transform.position)
					transitionToNewTarget = false;
			}
			else
				gameObject.transform.position = target.transform.position;
			//Bind to map limits
			if (this.transform.position.x + 7.5 > maxX)
			{
				this.transform.position = new Vector3(maxX - 7.5f, this.transform.position.y, -10f);
			} 
			else if (this.transform.position.x - 7.5 < minX)
				this.transform.position = new Vector3(minX + 7.5f, this.transform.position.y, -10f);
			if (this.transform.position.y - 5 < maxY)
			{
				this.transform.position = new Vector3(this.transform.position.x, maxY + 5f, -10f);
			} 
			else if (this.transform.position.y + 5 > minY)
				this.transform.position = new Vector3(this.transform.position.x, minY - 5f, -10f);
			
		}
	}
}