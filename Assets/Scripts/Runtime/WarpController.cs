using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WarpController : MonoBehaviour {
    public enum direction {up, down, left, right};
	public WarpController destinationWarp;
    public direction playerLookAtArrival;
    public bool insideMap;

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.tag == "Player")
		{
            StartCoroutine(WarpPlayer(coll.transform.parent));
        }
	}

    IEnumerator WarpPlayer(Transform player)
    {
        GameController.control.PauseGame();
        Color tmp;
        if (insideMap)
            tmp = Color.black;
        else
            tmp = Color.white;
        tmp.a = 0;
        GameController.control.guiController.transitionLayer.GetComponent<Image>().color = tmp;
        yield return GameController.control.guiController.FadeIn(1f);
        //position the player depending on which direction the warp is
        switch (destinationWarp.playerLookAtArrival)
        {
            case direction.up:
                player.position = new Vector3(destinationWarp.transform.position.x, destinationWarp.transform.position.y + 1.25f, 0);
                break;
            case direction.down:
                player.position = new Vector3(destinationWarp.transform.position.x, destinationWarp.transform.position.y - 1.25f, 0);
                break;
            case direction.right:
                player.position = new Vector3(destinationWarp.transform.position.x + 1.25f, destinationWarp.transform.position.y, 0);
                break;
            case direction.left:
                player.position = new Vector3(destinationWarp.transform.position.x - 1.25f, destinationWarp.transform.position.y, 0);
                break;
        }
        GameController.control.cameraController.ChangeMap(destinationWarp.transform.parent.gameObject);
        yield return StartCoroutine(GameController.control.guiController.FadeOut(1f));
        GameController.control.ResumeGame();
    }
}
