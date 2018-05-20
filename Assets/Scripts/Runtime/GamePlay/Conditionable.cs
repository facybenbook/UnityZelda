using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conditionable : MonoBehaviour
{
    public bool state;
    public List<Conditionable> conditions = new List<Conditionable>();
    public DoSomething action;

    protected virtual void Start()
    {
        state = false;
    }

    public virtual void OnCheckConditions()
    {
        if (conditions.Count > 0)
        {
            state = true;
            foreach (Conditionable trigger in conditions)
            {
                if (trigger == null)
                {
                    Debug.Log(gameObject.name + ", " + gameObject.transform.position + " condition not set");
                    state = false;
                }
                if (trigger.state == false)
                {
                    state = false;
                }
            }
            action();
        }
    }

    public delegate void DoSomething();
    
}