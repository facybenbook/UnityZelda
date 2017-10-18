using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public Transform target;
	public GameObject mapTarget;
	private bool transitionToNewTarget;
	public int speed;

	private float minX;
	private float minY;
	private float maxX;
	private float maxY;
	private float pixelUnit;
	// Use this for initialization
	void Start ()
	{
		pixelUnit = 0.625f;
		//boundaries from left top corner
		minX = mapTarget.transform.position.x * mapTarget.transform.lossyScale.x;
		maxX = (minX + mapTarget.GetComponent<Tiled2Unity.TiledMap> ().NumTilesWide) * mapTarget.transform.lossyScale.x;
		minY = mapTarget.transform.position.y * mapTarget.transform.lossyScale.y;
		maxY = (minY - mapTarget.GetComponent<Tiled2Unity.TiledMap> ().NumTilesHigh) * mapTarget.transform.lossyScale.x;
		speed = 6;
		target = GameObject.Find("Player").transform;
	}
	// Update is called once per frame
	void Update ()
	{
		Move ();
	}

	public void ChangeTarget (GameObject newTarget, bool transition)
	{
		if (newTarget != null) 
		{
			target = newTarget.transform;
			transitionToNewTarget = transition;
		}
	}
	public void ChangeMap (GameObject newMap)
	{
        if (newMap != null)
        {
            mapTarget = newMap;
            minX = mapTarget.transform.position.x;
            maxX = (minX + mapTarget.GetComponent<Tiled2Unity.TiledMap>().NumTilesWide);
            minY = mapTarget.transform.position.y;
            maxY = (minY - mapTarget.GetComponent<Tiled2Unity.TiledMap>().NumTilesHigh);
        }
        else
        {
            minX = 0;
            maxX = 15;
            minY = 0;
            maxY = 10;
        }
	}

	private void Move ()
	{
		if (transitionToNewTarget) {
			transform.position = Vector3.MoveTowards (transform.position, target.position, pixelUnit);
			if (transform.position == target.position)
				transitionToNewTarget = false;
		} else if (target != null) {
			transform.position = target.position;
			//Bind to map limits
			if (transform.position.x + 7.5 > maxX) {
				transform.position = new Vector3 (maxX - 7.5f, transform.position.y, 0);
			} else if (transform.position.x - 7.5 < minX)
				transform.position = new Vector3 (minX + 7.5f, transform.position.y, 0);
			if (transform.position.y - 5 < maxY) {
				transform.position = new Vector3 (transform.position.x, maxY + 5f, 0);
			} else if (transform.position.y + 5 > minY)
				transform.position = new Vector3 (transform.position.x, minY - 5f, 0);
		}
	}
}