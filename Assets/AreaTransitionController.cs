using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransitionController : MonoBehaviour
{
    public MapArea firstArea;
    public MapArea secondArea;
    public int transitionAreaSize;

    // Use this for initialization
    void Start()
    {

    }

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
        yield return (player.GetComponent<PlayerController>().Walk(transitionAreaSize));
        if (GameController.control.cameraController.targetArea == firstArea)
            GameController.control.cameraController.TransitionArea(secondArea);
        else
            GameController.control.cameraController.TransitionArea(firstArea);
        GameController.control.ResumeGame();
    }
}
