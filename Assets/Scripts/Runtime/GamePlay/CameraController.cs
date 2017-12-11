using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public Transform target;
	public MapArea targetArea;
	private bool transitionToNewTarget;
    private bool transitionArea;
	public int speed;

	public Vector2 min;
	public Vector2 max;
	private float pixelUnit;
	// Use this for initialization
	void Start ()
	{
		pixelUnit = 0.625f;
        if (targetArea != null)
        {
            //boundaries from left top corner
            GetBoundaries();
        }
        else
        {
            Debug.LogWarning("boundary area not set for the camera");
        }
		speed = 6;
        if (target == null)
		    target = GameObject.Find("Player").transform;
	}
	// Update is called once per frame
	void FixedUpdate ()
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
	public void ChangeMap (MapArea newArea)
	{
        if (newArea != null)
        {
            targetArea = newArea;
            GetBoundaries();
        }
        else
        {
            min.x = 0;
            max.x = 15;
            min.y = 0;
            max.y = 10;
        }
	}

    public void TransitionArea(MapArea newArea)
    {
        transitionArea = true;
        targetArea = newArea;
    }

	private void Move ()
	{
        if (transitionArea)
        {
            if (targetArea)
            {
                transform.position = target.position;
                if (transform.position == BindToArea())
                    transitionArea = false;
            }
        }
		else if (transitionToNewTarget) {
			transform.position = Vector3.MoveTowards (transform.position, target.position, pixelUnit);
			if (transform.position == target.position)
				transitionToNewTarget = false;
		}
        else if (target != null) {
			transform.position = target.position;
			//Bind to map limits
            if (targetArea)
            {
                transform.position = BindToArea();
            }
		}
	}
    
    private Vector3 BindToArea()
    {
        Vector3 binding = transform.position;

        if (transform.position.x + 7.5f > max.x || transform.position.x - 7.5f < min.x)
            binding.x = transform.position.x + 7.5f > max.x ? max.x - 7.5f : min.x + 7.5f;
        if (transform.position.y + 5 > min.y || transform.position.y - 5 < max.y)
            binding.y = transform.position.y + 5 > min.y ? min.y - 5f : max.y + 5f;
        return binding;
    }

    private void GetBoundaries()
    {
        min.x = targetArea.transform.position.x;
        max.x = (min.x + targetArea.area.x);
        min.y = targetArea.transform.position.y;
        max.y = (min.y - targetArea.area.y);
    }
}