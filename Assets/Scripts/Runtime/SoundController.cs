using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController : MonoBehaviour 
{
	private AudioSource playerSource;
	private bool alreadyPlaying;
	private bool playOnce;
	public List<string> playingList;
	public bool playing;
	public AudioClip swordSound;
	public AudioClip chargeSound;
	public AudioClip spinSound;
	public AudioClip[] linkSwordSounds = new AudioClip[5];
	// Use this for initialization
	void Start () {
		playingList = new List<string> ();
		playerSource = gameObject.AddComponent<AudioSource> ();
		spinSound = 	(AudioClip)Resources.Load ("AudioClip/SoundEffects/Link_SwordSpin");
		chargeSound = 	(AudioClip)Resources.Load ("AudioClip/SoundEffects/Link_ChargeSword");
		swordSound = 	(AudioClip)Resources.Load ("AudioClip/SoundEffects/Link_Sword");
		linkSwordSounds = new AudioClip[]{
						(AudioClip)Resources.Load ("AudioClip/SoundEffects/Link_SwordSound1"),
						(AudioClip)Resources.Load ("AudioClip/SoundEffects/Link_SwordSound2"),
						(AudioClip)Resources.Load ("AudioClip/SoundEffects/Link_SwordSound3")
						};
	}

	public void Update () 
	{
		if (playing == true) {
			if (GetComponent<Animator> ().GetBool ("is_sword")) {
				if (!playingList.Contains ("is_sword")) {
					playerSource.PlayOneShot (swordSound);
					playerSource.PlayOneShot (linkSwordSounds [Random.Range (0, 3)]);
					playingList.Add ("is_sword");
				}
			}
			if (GetComponent<Animator> ().GetBool ("is_charging")) {
				if (!playingList.Contains ("is_charging")) {
					playerSource.PlayOneShot (chargeSound);
					playingList.Add ("is_charging");
				}
			}
			if (GetComponent<Animator> ().GetBool ("is_tornado")) {	
				if (!playingList.Contains ("is_tornado")) {
					playerSource.PlayOneShot (spinSound);
					playingList.Add ("is_tornado");
				}
			}
			playing = false;
		} else {
			//remove all states that are no longer activated
			if (playingList.Count > 0)
				playingList.RemoveAll (x => !GetComponent<Animator> ().GetBool (x));
		}
		
	}
}
