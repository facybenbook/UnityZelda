using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LogicMechanism : MonoBehaviour
{
    protected bool state = false;
    public List<LogicMechanism> activationConditions = new List<LogicMechanism>();
    public Action onActivate;
    public Action onDeactivate;

    public virtual void Start()
    {
        if (activationConditions == null)
            Debug.LogWarning("activation condition is empty !");
    }

    public virtual void SetState(bool newState)
    {
        state = newState;
    }

    public virtual void ActivateOrDeactivate()
    {
        bool newState = CheckConditions(activationConditions);
        if (newState != state)
        {
            SetState(newState);
            if (state == true)
            {
                if (onActivate != null)
                    onActivate.Invoke();
            }
            else
            {
                if (onDeactivate != null)
                    onDeactivate.Invoke();
            }
        }
    }

    /// <summary>
    /// Chcks the conditions set for this mechanism
    /// </summary>
    /// <param name="conditions"></param>
    /// <returns>returns true if no condition is set or all conditions are met</returns>
    protected virtual bool CheckConditions(List<LogicMechanism> conditions)
    {
        bool result = true;
        if (conditions != null && conditions.Count > 0)
        {
            print("ok");
            foreach (LogicMechanism condition in conditions)
            {
                //if the condition is empty
                if (condition == null)
                {
                    Debug.LogWarning(gameObject.name + ", " + gameObject.transform.position + " activation condition not set");
                }
                else if (condition.state == false)
                {
                    result = false;
                }
            }
        }
        return result;

    }
}