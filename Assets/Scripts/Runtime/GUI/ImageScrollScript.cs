using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageScrollScript : MonoBehaviour 
{
	public Vector2 scrollVector = new Vector2( -1.0f, -1.0f );
	public float speed = 1;
	private Image image;
	private  Vector2 uvOffset = Vector2.zero;

	void Start()
	{
		image = gameObject.GetComponent<Image> ();
	}
	void LateUpdate() 
	{
		uvOffset += ( scrollVector * speed * Time.deltaTime);
		if(image.enabled)
		{
			image.material.SetTextureOffset ("_MainTex", uvOffset);
		}
	}


}
