using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class MessageBox : MonoBehaviour
{
	public int numberOfLines;
	public int maxCharsOnLine;
	bool messageDisplayed;
	public GameObject messagePanel;
	public GameObject cursor;
	private Text textGUI;
	private int indexInText;
	public int charsOnLine;
	private int lineNumber;

	void Start ()
	{
		messageDisplayed = false;
		cursor = transform.FindChild ("Cursor").gameObject;
		messagePanel = gameObject;
		textGUI = transform.FindChild ("Mask/Text").GetComponentInChildren<Text> ();
	}

	public void DisplayMessage (string text)
	{
		if (messageDisplayed == false) {
			textGUI.text = "";
			messageDisplayed = true;
			StartCoroutine (DisplayOneByOne (text));
		}
	}

	IEnumerator DisplayOneByOne (string message)
	{
		charsOnLine = 0;
		lineNumber = 0;
		string[] words;
		GameController.control.PauseGame ();
		cursor.GetComponent<Image> ().enabled = false;
		messagePanel.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, 0, 0);
		textGUI.rectTransform.anchoredPosition = new Vector3 (0, 0, 0);
		textGUI.text = "";
//		while(lineNumber < numberOfLines)
//		{
		MatchCollection matches = Regex.Matches (message, "(\\s|\\<.*?\\>|[^\\<\\s]+)");
		words = new string[matches.Count];
		for (int i = 0; i < matches.Count; i++) {
			words [i] = matches [i].ToString ();
		}
		indexInText = 0;
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
					yield return CheckEndOfLine (words [j]);
					yield return AddLetters (words [j]);
					yield return CheckIfNextLine ();
				}
				i += untilMatching;
				indexInText += words [i].Length;
			} else {
				yield return CheckEndOfLine (words [i]);
				yield return AddLetters (words [i]);
				yield return CheckIfNextLine ();
			}
		}
		//debug
		cursor.GetComponent<Image> ().enabled = true;
		while (!Input.GetKeyDown (KeyCode.Return))
			yield return null;
		messagePanel.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, -55, 0);
		messageDisplayed = false;
		GameController.control.ResumeGame ();
	}

	IEnumerator CheckIfNextLine ()
	{
		if (lineNumber > 1) {
			cursor.GetComponent<Image> ().enabled = true;
			while (!Input.GetKeyDown (KeyCode.Return)) {
				yield return null;
			}
			cursor.GetComponent<Image> ().enabled = false;
			//while (textGUI.rectTransform.anchoredPosition.y != textGUI.rectTransform.anchoredPosition.y + 16) {
			textGUI.rectTransform.anchoredPosition = new Vector3 (0, textGUI.rectTransform.anchoredPosition.y + 15, 0);
			//}
			lineNumber--;
		}
	}

	IEnumerator CheckEndOfLine (string word)
	{
		if (charsOnLine + word.Length >= maxCharsOnLine) {
			if (word [0] != ' ') {
				textGUI.text = textGUI.text.Insert (indexInText++, '\n'.ToString ());
				charsOnLine = 0;
				lineNumber++;
				yield return CheckIfNextLine ();
			}
		}
	}

	IEnumerator AddLetters (string word)
	{
		char[] tmp = word.ToCharArray ();
		//if empty word, add space
		if (tmp.Length == 1 && tmp [0] == ' ') {
			if (maxCharsOnLine != 0) {
				textGUI.text = textGUI.text.Insert (indexInText++, ' '.ToString ());
				charsOnLine++;
			}
		} else {
			for (int j = 0; j < tmp.Length; j++) {
				textGUI.text = textGUI.text.Insert (indexInText++, tmp [j].ToString ());
				yield return WaitForRealTime (1 / 60f);
			}
			charsOnLine += word.Length;
		}
	}

	public static IEnumerator WaitForRealTime (float delay)
	{
		while (true) {
			float pauseEndTime = Time.realtimeSinceStartup + delay;
			while (Time.realtimeSinceStartup < pauseEndTime) {
				yield return 0;
			}
			break;
		}
	}
}
