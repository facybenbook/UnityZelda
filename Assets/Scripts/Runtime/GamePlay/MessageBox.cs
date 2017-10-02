using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class MessageBox : MonoBehaviour
{
	public int numberOfLines;
	public int maxCharacterPixelsOnLine;
	public bool messageDisplayed;
	public GameObject messagePanel;
	public GameObject cursor;
	private Text textGUI;
	private int indexInText;
	private float characterPixelsOnLine;
	private int lineNumber;
	private float[] charsSize;
	private bool fastForward;
	void Start ()
	{
		messageDisplayed = false;
		cursor = transform.Find ("Cursor").gameObject;
		messagePanel = gameObject;
		textGUI = transform.Find ("Mask/Text").GetComponentInChildren<Text> ();
		maxCharacterPixelsOnLine = (int)textGUI.gameObject.GetComponent<RectTransform> ().rect.width;
		characterPixelsOnLine = 0;
	}

	public IEnumerator DisplayMessage (string text)
	{
		if (messageDisplayed == false) {
			messageDisplayed = true;
			yield return DisplayOneByOne (text);
		}
	}

	IEnumerator DisplayOneByOne (string message)
	{
		characterPixelsOnLine = 0;
		lineNumber = 0;
		string[] words;
		GameController.control.PauseGame ();
		cursor.GetComponent<Image> ().enabled = false;
		messagePanel.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, 0, 0);
		textGUI.rectTransform.anchoredPosition = new Vector3 (0, 0, 0);
		textGUI.text = "";
		MatchCollection matches = Regex.Matches (message, "(\\s|\\<.*?\\>|[^\\<\\s]+)");
		words = new string[matches.Count];
		for (int i = 0; i < matches.Count; i++) {
			words [i] = matches [i].ToString ();
		}
		indexInText = 0;
		fastForward = false;
		StartCoroutine (FastForward ());
		for (int i = 0; i < words.Length; i++) {
			if (words [i].IndexOf (('<')) == 0) {
				int untilMatching = 0;
				indexInText += words [i].Length;
				textGUI.text += words [i++];
				while (words [i + untilMatching].IndexOf (('<')) != 0) {
					untilMatching++;
				}
				//add the closing tag
				textGUI.text += words [i + untilMatching];
				//fill the middle
				for (int j = i; j < i + untilMatching; j++) {
					yield return AddLetters (words [j]);
				}
				i += untilMatching;
				indexInText += words [i].Length;
			} else {
				yield return AddLetters (words [i]);
			}
		}
		cursor.GetComponent<Image> ().enabled = true;
		StopCoroutine (FastForward ());
		yield return WaitForInput (GameKeys.ENTER);
		messagePanel.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -55, 0);
		messageDisplayed = false;
	}

	IEnumerator FastForward()
	{
		while (true) {
			if (!fastForward) {
				yield return WaitForInput (GameKeys.ENTER);
				fastForward = true;
			}
			else
				yield return null;
		}	
	}

	IEnumerator CheckIfNextLine ()
	{
		if (lineNumber > 1) {
			cursor.GetComponent<Image> ().enabled = true;
			yield return WaitForInput (GameKeys.ENTER);
			cursor.GetComponent<Image> ().enabled = false;
			textGUI.rectTransform.anchoredPosition = new Vector3 (0, textGUI.rectTransform.anchoredPosition.y + 15, 0);
			lineNumber--;
			fastForward = false;
		}
	}
		
	int WordLengthInPixels(string word)
	{
		int length = 0;
		CharacterInfo character;
		if (word.Length == 1 && word [0] == ' ')
			return 4;
		for (int i = 0; i < word.Length; i++) {
			textGUI.font.GetCharacterInfo (word [i], out character);
			length += character.glyphWidth;
			//add one pixel for the letter spacing
			if (i < word.Length - 1)
				length++;
		}
		return (length);
	}

	IEnumerator CheckEndOfLine (string word)
	{
		//<debug>
//		print (word + ":\t" +characterPixelsOnLine +":+\t" + WordLengthInPixels(word) + ":\t" + maxCharacterPixelsOnLine);
		//<\debug>
		if (characterPixelsOnLine + WordLengthInPixels(word) > maxCharacterPixelsOnLine) {
			if (word [0] != ' ') {
				textGUI.text = textGUI.text.Insert (indexInText++, '\n'.ToString ());
				characterPixelsOnLine = 0;
				lineNumber++;
				yield return CheckIfNextLine ();
			}
		}
	}

	IEnumerator AddLetters (string word)
	{
		yield return CheckEndOfLine (word);
		CharacterInfo infos;
		char[] tmp = word.ToCharArray ();
		//if empty word, add space
		if (tmp.Length == 1 && tmp [0] == ' ') {
			if (maxCharacterPixelsOnLine != 0) {
				textGUI.text = textGUI.text.Insert (indexInText++, ' '.ToString ());
				textGUI.font.GetCharacterInfo (tmp [0], out infos);
				characterPixelsOnLine += 4;
			}
		}
		else {
			for (int j = 0; j < tmp.Length; j++) {
				textGUI.text = textGUI.text.Insert (indexInText++, tmp [j].ToString ());
				if (fastForward == false)
					yield return Wait (1 / 60f);
				textGUI.font.GetCharacterInfo (tmp [j], out infos);
				characterPixelsOnLine += infos.glyphWidth; 
				//add one pixel for the lette spacing
				if (j < tmp.Length - 1)
					characterPixelsOnLine++;
			}
		}
	}

	IEnumerator WaitForInput(KeyCode key)
	{
		while (!Input.GetKeyDown (key)) {
			yield return null;
		}
	}

	IEnumerator Wait(float time)
	{
		yield return new WaitForSeconds(time);
	}
}
