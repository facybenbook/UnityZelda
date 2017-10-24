using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conditionable : MonoBehaviour
{
    public bool state;
    public bool visible = true;
    public List<Conditionable> conditions = new List<Conditionable>();
    public DoSomething action;
    void Start()
    {
        action = Nothing;
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
                    action();
                    state = true;
                }
            }
        }
    }

    public delegate void DoSomething();

    void Nothing()
    { }
}