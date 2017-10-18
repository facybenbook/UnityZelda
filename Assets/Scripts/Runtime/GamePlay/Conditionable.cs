using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conditionable : MonoBehaviour
{
    public bool state;
    public bool visible = true;
    public List<Conditionable> conditions = new List<Conditionable>();
    void Start()
    {
        state = false;
    }

    public void OnCheckConditions()
    {
        if (visible == false)
        {
            if (state == false)
            {
                if (conditions.Count > 0)
                {
                    foreach (Conditionable trigger in conditions)
                    {
                        if (trigger == null)
                        {
                            Debug.Log(gameObject.name + ", " + gameObject.transform.position + " condition not set");
                            return;
                        }
                        if (trigger.state == false)
                            return;
                    }
                    DoSomething();
                    state = true;
                }
            }
        }
    }

    protected virtual void DoSomething()
    { }
}