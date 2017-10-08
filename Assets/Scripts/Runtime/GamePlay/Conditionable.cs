using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conditionable : MonoBehaviour
{
    public bool state;
    public List<Conditionable> conditions = new List<Conditionable>();
    void Start()
    {
        state = false;
    }

    public void OnCheckConditions()
    {
        if (state == false)
        {
            foreach (Conditionable trigger in conditions)
            {
                if (trigger.state == false)
                    return;
            }
            DoSomething();
            state = true;
        }
    }

    protected virtual void DoSomething()
    { }
}