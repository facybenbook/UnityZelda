using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ElementsBarScript : MonoBehaviour {

	public GameObject element;
	public Sprite[] elementSprites = new Sprite[6];
	private List<GameObject> elements;

	// Use this for initialization
	void Start () {
		elements = new List<GameObject>();
		for(int i = 0; i < elementSprites.Length; i++)
		{
			this.AddElement(i);
			elements[i].SetActive (false);
		}
	}
	public void UpdateInfos (PlayerStats infos)
	{
		for (int i = 0; i < infos.elements.Length; i++) {
			if (elements [i].activeSelf != infos.elements[i]) {
				elements [i].SetActive (infos.elements[i]);
			}
		}
	}

	private void AddElement(int Number)
	{
		GameObject newElement = GameObject.Instantiate(element);
		elements.Add(newElement);
		newElement.transform.SetParent(this.transform);
		newElement.name = "Element" + Number;
		newElement.GetComponent<RectTransform>().anchoredPosition = new Vector3(Number % 10 * 20, Number / 10 * -16, 0f);
		newElement.GetComponent<Image>().sprite = elementSprites[Number];
	}
}
