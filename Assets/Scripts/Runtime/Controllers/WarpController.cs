using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WarpController : MonoBehaviour {

    public enum direction {up, down, left, right};
	public WarpController destinationWarp;
    public MapArea area;
    public direction playerLookAtArrival;
    public bool insideMap;

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.tag == "Player")
		{
            StartCoroutine(WarpPlayer(coll.transform));
        }
	}

    IEnumerator WarpPlayer(Transform player)
    {
        GameController.control.PauseGame();
        Color tmp;
        //color of the transition layer
        if (destinationWarp.insideMap)
            tmp = Color.black;
        else
            tmp = Color.white;
        tmp.a = 0;
        GameController.control.guiController.transitionLayer.GetComponent<Image>().color = tmp;
        yield return GameController.control.guiController.FadeIn(0.2f);
        if (destinationWarp.area.map != area.map)
            destinationWarp.area.map.SetActive(true);
        GameController.control.cameraController.ChangeMap(destinationWarp.area);
        player.SetParent(destinationWarp.transform.parent);
        //position the player depending on which direction the warp is
        switch (destinationWarp.playerLookAtArrival)
        {
            case direction.up:
                player.position = new Vector3(destinationWarp.transform.position.x, destinationWarp.transform.position.y + 1.25f, 0);
                player.GetComponent<PlayerController>().CharacterOrientation = Vector2.up;
                break;
            case direction.down:
                player.position = new Vector3(destinationWarp.transform.position.x, destinationWarp.transform.position.y - 1.25f, 0);
                player.GetComponent<PlayerController>().CharacterOrientation = Vector2.down;
                break;
            case direction.right:
                player.position = new Vector3(destinationWarp.transform.position.x + 1.25f, destinationWarp.transform.position.y, 0);
                player.GetComponent<PlayerController>().CharacterOrientation = Vector2.right;
                break;
            case direction.left:
                player.position = new Vector3(destinationWarp.transform.position.x - 1.25f, destinationWarp.transform.position.y, 0);
                player.GetComponent<PlayerController>().CharacterOrientation = Vector2.left;
                break;
            default:
                break;
        }
        yield return StartCoroutine(GameController.control.guiController.FadeOut(0.2f));
        if (destinationWarp.area.map != area.map)
            area.map.SetActive(false);
        GameController.control.ResumeGame();
    }
}
