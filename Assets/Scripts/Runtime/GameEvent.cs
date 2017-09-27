//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using UnityEngine.Events;
//using UnityEngine;
//using System;
//
//[Serializable]
//public enum ContactWith {None, All, Player, Monster, Weapon, Arrow, Bomb, Sword, prout, ahah, lol}
//
//[Serializable]
//public class GameEvent : MonoBehaviour 
//{
//	
//	[SerializeField]
//	private bool none;
//	[SerializeField]
//	private bool all;
//	public bool[] triggersOn = new bool[2];
//	public bool destroyAfterAction;
//	public UnityEvent onTriggerCommands;
////	public UnityEvent onUpdateCommands;
//	public static string[] activatorNames;
//
//	public void Init()
//	{
//		destroyAfterAction = true;
//		if (activatorNames == null) {
//			activatorNames = System.Enum.GetNames (typeof(ContactWith));
//		}
//		int enumLength = GameEvent.activatorNames.Length;
//		if (triggersOn.Length != enumLength) {
//			Array.Resize (ref triggersOn, enumLength);
//		}
//	}
//	void Awake()
//	{
//		Init ();
//	}
//
//	void OnValidate ()
//	{
//		if (triggersOn[0]) {
//			for (int i = 0; i < triggersOn.Length; i++)
//			{
//				triggersOn[i] = false;
//			}
//			triggersOn[0] = false;
//		}
//		if (triggersOn[1])
//		{
//			for (int i = 0; i < triggersOn.Length; i++)
//			{
//				triggersOn[i] = true;
//			}
//			triggersOn[0] = false;
//		}
//      }
//	void Update () {
////		if (onUpdateCommands != null)
////			onUpdateCommands.Invoke();
//	}
//	void Trigger ()
//	{
//		onTriggerCommands.Invoke ();
//		if (destroyAfterAction)
//			Destroy(this.gameObject);
//	}
//
//	void OnTriggerEnter2D(Collider2D coll)
//	{
//	//	Debug.Log ("Triggered!" + coll.name);
//		for (int i = 2; i < triggersOn.Length; i++) {
//			if (coll.gameObject.tag != activatorNames[i]) {
//				return;
//			}
//		}
//		Trigger ();
//	}
//	void OnCollisionEnter2D(Collision2D coll)
//	{
//	//	Debug.Log ("Bump");
//		for (int i = 2; i < triggersOn.Length; i++) {
//			if (coll.gameObject.tag != activatorNames[i]) {
//				return;
//			}
//		}
//		Trigger ();
//	}
//	public void Move (float speed, Vector2 direction)
//	{
//
//	}
//	public void AddRupees (int amount)
//	{
//		GameController.control.AddRupees (amount);
//	}
//	public void ChangeSprite (Sprite image)
//	{
//		GetComponent<SpriteRenderer> ().sprite = image;
//	}
//	public void NewHeartContainer()
//	{
//		GameController.control.NewHeart ();
//	}
//	public IEnumerator NewHeartPiece()
//	{	
//		yield return GameController.control.NewHeartPiece ();
//	}
//	public IEnumerator ChangeRupeeStash(string size)
//	{
//		GameController.control.ChangeRupeeStash (size);
//		yield return null;
//	}
//
//}
