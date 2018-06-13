using System;
using System.Collections.Generic;
using UnityEngine;

// requires the GameObject to have a SpriteRenderer component attached
[RequireComponent(typeof(SpriteRenderer))]
public class HideableMechanism : LogicMechanism
{
    public bool visible = true;
    //for specific actions on appearing and disapearing
    public Action onAppear;
    public Action onDisappear;
    public List<LogicMechanism> appearingConditions = new List<LogicMechanism>();
    
    protected virtual void Start()
    {
        if (appearingConditions == null)
            Debug.LogWarning("appearing condition is empty !");
        if (visible == false)
        {
            SetVisible(false);
        }
    }

    public override void ActivateOrDeactivate()
    {
        HideOrShow();
        base.ActivateOrDeactivate();
    }

    public void SetVisible(bool isVisible)
    {
        visible = isVisible;
        //not hidden is visible
        GetComponent<SpriteRenderer>().enabled = visible;
        GetComponent<BoxCollider2D>().enabled = visible;
    }
    
    public void HideOrShow()
    {
        bool isVisible = CheckConditions(appearingConditions);
        //if not already visible or invisible
        if (isVisible != visible)
        {
            SetVisible(isVisible);
            if (visible == true)
            {
                if (onAppear != null)
                    onAppear.Invoke();
            }
            else
            {
                if (onDisappear != null)
                    onDisappear.Invoke();
            }
        }
    }
}

