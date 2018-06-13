using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Orientation { Vertical, Horizontal }
public enum FadeTransition { None, White, Black }
public class RoomChanger : MonoBehaviour {

    private bool activated = false;
    public FadeTransition fadedTransition = FadeTransition.None;
    public Orientation orientation = Orientation.Vertical;
    public CinemachineVirtualCamera sourceCamera;
    public CinemachineVirtualCamera destinationCamera;
    public float distance;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision" + collision);
        Debug.Log(GameController.control.player);
        if (collision.tag == "Player" && activated == false)
        {
            activated = true;
            StartCoroutine(Activate(collision.GetComponent<PlayerController>()));
            activated = false;
        }
    }
    
    public IEnumerator Activate(PlayerController player)
    {
        Vector2 direction;
        var box1 = sourceCamera.gameObject.GetComponent<CinemachineConfiner>().m_BoundingShape2D;
        var box2 = destinationCamera.gameObject.GetComponent<CinemachineConfiner>().m_BoundingShape2D;
        if (orientation == Orientation.Vertical)
        {
            if (sourceCamera.gameObject.activeSelf)
                direction = (box1.transform.position.y > box2.transform.position.y) ? Vector2.down : Vector2.up;
            else
                direction = (box1.transform.position.y > box2.transform.position.y) ? Vector2.up : Vector2.down;
        }
        else
        {
            if (sourceCamera.gameObject.activeSelf)
                direction = (box1.transform.position.x > box2.transform.position.x) ? Vector2.left : Vector2.right;
            else
                direction = (box1.transform.position.x > box2.transform.position.x) ? Vector2.right : Vector2.left;
        }
        player.IsBusy = true;
        if (fadedTransition != FadeTransition.None)
        {
            Color color = (fadedTransition == FadeTransition.White) ? Color.white : Color.black;
           yield return GameController.control.guiController.FadeIn(color, 0.2f);
        }
        yield return player.Move(distance, direction);
        player.IsBusy = false;
        SwitchCameras();

        if (fadedTransition != FadeTransition.None)
           yield return GameController.control.guiController.FadeOut(0.2f);
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator WaitForPysicsFrames(int number)
    {
        while (number > 0)
        {
            number--;
            yield return new WaitForFixedUpdate();
        }
    }

    void SwitchCameras()
    {
        if (sourceCamera.gameObject.activeSelf)
        {
            destinationCamera.gameObject.SetActive(true);
            sourceCamera.gameObject.SetActive(false);
        }
        else
        {
            destinationCamera.gameObject.SetActive(false);
            sourceCamera.gameObject.SetActive(true);
        }
    }

}
