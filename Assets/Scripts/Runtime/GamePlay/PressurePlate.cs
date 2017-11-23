using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Conditionable))]
public class PressurePlate : Conditionable {
    public Conditionable conditionable;

    protected override void Start()
    {
        base.Start();
        if (conditionable == null)
            conditionable = GetComponent<Conditionable>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == false)
        {
            if (collision.isTrigger == false)
            {
                conditionable.ChangeState(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //return to inactive state if something is not on the plate
        if (conditionable.staysActivated == false && collision.isTrigger == false)
        {
            conditionable.ChangeState(false);
        }
    }
}
