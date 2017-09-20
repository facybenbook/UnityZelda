using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject 
{
	public Sprite sprite;
	public enum type {Rupee, Stash, Heart, Item};
	public GameObject itemObject;

}
