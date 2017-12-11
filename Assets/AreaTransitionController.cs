using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransitionController : MonoBehaviour
{
    public MapArea firstArea;
    public MapArea secondArea;
    public int transitionAreaSize;

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
        if (GameController.control.cameraController.targetArea == firstArea)
            GameController.control.cameraController.TransitionArea(secondArea);
        else
            GameController.control.cameraController.TransitionArea(firstArea);
        yield return (player.GetComponent<PlayerController>().Walk(transitionAreaSize));
        GameController.control.ResumeGame();
    }
}
