using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementPattern {FollowTarget, ImitateTarget, MirrorTarget, FollowWalls, Random}
public enum MovementType {Normal, XOnly, YOnly, Axial, Diagonal, Charge}
public class PNJController : ActorController {

    public MovementPattern movementPattern;
    Pattern pattern;
    public Transform movementTarget;
    public float frequency;
    public MovementType movementType;
    Type type;

    protected override void Start()
    {
        if (pattern == null)
        {
            switch (movementPattern)
            {
                case MovementPattern.FollowTarget:
                    pattern = FollowTarget;
                    break;
                case MovementPattern.ImitateTarget:
                    pattern = ImitateTarget;
                    break;
                case MovementPattern.MirrorTarget:
                    pattern = MirrorTarget;
                    break;
                case MovementPattern.Random:
                    pattern = RandomMovement;
                    break;
            }
        }
        if (type == null)
        {
            switch (movementType)
            {
                case MovementType.Normal:
                    type = Normal;
                    break;
                case MovementType.XOnly:
                    type = XOnly;
                    break;
                case MovementType.YOnly:
                    type = YOnly;
                    break;
                case MovementType.Axial:
                    type = Axial;
                    break;
                case MovementType.Diagonal:
                    type = Diagonal;
                    break;
                case MovementType.Charge:
                    type = Charge;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update() {
    }

    delegate void Pattern();
    delegate void Type();

    //movement patterns
    void FollowTarget()
    {

    }

    void ImitateTarget()
    {

    }

    void MirrorTarget()
    {

    }

    void RandomMovement()
    {

    }

    //movement types
    void Normal()
    {

    }

    void XOnly()
    {

    }

    void YOnly()
    {

    }

    void Axial()
    {

    }

    void Diagonal()
    {

    }

    void Charge()
    {

    }

}
