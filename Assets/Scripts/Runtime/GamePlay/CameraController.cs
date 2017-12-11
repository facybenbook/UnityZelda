using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public Transform target;
	public MapArea targetArea;
	private bool transitionToNewTarget;
    private bool transitionArea;
    private bool cameraBinding = true;
	public int speed = 6;

	public Vector2 min;
	public Vector2 max;
	private float pixelUnit = 0.625f;
	// Use this for initialization
	void Start ()
	{
        if (target == null)
		    target = GameObject.Find("Player").transform;
    }
	// Update is called once per frame
	void FixedUpdate ()
	{
        if ((max == Vector2.zero || min == Vector2.zero) && targetArea)
        {
            //boundaries from left top corner
            GetBoundaries();
        }
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
        cameraBinding = false;
        ChangeMap(newArea);
    }

	private void Move ()
	{
        if (cameraBinding)
        {
            if (target)
            {
                if (transitionToNewTarget)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, pixelUnit);
                    if (transform.position == target.position)
                        transitionToNewTarget = false;
                }
                else
                {
                    transform.position = target.position;
                }
                //Bind to map limits
                if (targetArea)
                {
                    transform.position = BindToArea();
                }
            }
        }
        else if (transitionArea && targetArea)
        {
            print(BindToArea());
            transform.position = Vector3.MoveTowards(transform.position, BindToArea(), pixelUnit);
            if (transform.position == BindToArea())
            {
                transitionArea = false;
                cameraBinding = true;
            }
        }

    }
    
    private Vector3 BindToArea()
    {
        Vector3 binding = transform.position;
        if (transform.position.x + 7.5f > max.x || transform.position.x - 7.5f < min.x)
            binding.x = transform.position.x + 7.5f > max.x ? max.x - 7.5f : min.x + 7.5f;
        if (transform.position.y + 5 > max.y || transform.position.y - 5 < min.y)
            binding.y = transform.position.y + 5 > max.y ? max.y - 5 : min.y + 5;
        return binding;
    }

    private void GetBoundaries()
    {
        //goes in the direction: /
        min.x = targetArea.transform.position.x;
        max.x = (min.x + targetArea.area.x);
        max.y = targetArea.transform.position.y;
        min.y = (max.y - targetArea.area.y);
    }
}