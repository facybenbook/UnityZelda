using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Conditionable : MonoBehaviour {
    
    public bool state;
    public bool startsHidden;
    private bool visible;
    public List<Conditionable> appearConditions;
    public List<Conditionable> conditions = new List<Conditionable>();
    public bool staysActivated;
    public float activationTime;
    public Animator anim;
    public Sprite activatedSprite;
    public Sprite deactivatedSprite;
    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;

    protected virtual void Start()
    {
        state = false;
        
        //initialize the animator
        if (GetComponent<Animator>())
        {
            anim = GetComponent<Animator>();
            GetComponent<Animator>().SetBool("Active", false);
        }
        //else set the spriteRenderer
        else if (deactivatedSprite == null)
            deactivatedSprite = GetComponent<SpriteRenderer>().sprite;
        if (startsHidden)
            ToggleVisible(false);
    }

    public void OnCheckConditions()
    {
        if (startsHidden)
        {
            bool result = true;

            foreach (Conditionable trigger in appearConditions)
            {
                if (trigger == null)
                {
                    Debug.Log(gameObject.name + ", " + gameObject.transform.position + " condition not set");
                    result = false;
                }
                if (trigger.state == false)
                {
                    result = false;
                }
            }
            if (visible != result)
            {
                ToggleVisible(true);
                //won't evaluate the conditions in the future
                startsHidden = false;
            }
        }
        //if there are conditions to check
        else if (conditions.Count > 0)
        {
            bool result = true;
            foreach (Conditionable trigger in conditions)
            {
                if (trigger == null)
                {
                    Debug.Log(gameObject.name + ", " + gameObject.transform.position + " condition not set");
                    result = false;
                }
                if (trigger.state == false)
                {
                    result = false;
                }
            }
            if (result != state)
            {
                ChangeState(result);
            }
        }
    }

    public void ToggleVisible(bool newVisible)
    {
        //enable/disable every graphic component and collider of the object if its not visible
        visible = newVisible;
        GetComponent<SpriteRenderer>().enabled = visible;
        if (anim)
            anim.enabled = visible;
        BoxCollider2D[] tmp = GetComponents<BoxCollider2D>();
        foreach(BoxCollider2D coll in tmp)
        {
            coll.enabled = visible;
        }
    }

    public void ChangeState(bool newState)
    {
        state = newState;
        if (anim)
        {
            anim.SetBool("Active", state);
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = state ? activatedSprite : deactivatedSprite;
        }
        GetComponent<AudioSource>().Play();
        foreach (Conditionable obj in FindObjectsOfType<Conditionable>())
        {
            obj.gameObject.SendMessage("OnCheckConditions", SendMessageOptions.DontRequireReceiver);
        }
    }
}
