using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PNJController))]
public class PNJControllerEditor : Editor {

    PNJController myTarget;

	void OnEnable()
    {
        if (!myTarget)
        myTarget = (PNJController)target;
    }

    public override void OnInspectorGUI()
    {
        myTarget.movementPattern = (MovementPattern)EditorGUILayout.EnumPopup("Movement Pattern", myTarget.movementPattern);
        if (myTarget.movementPattern != MovementPattern.Random && myTarget.movementPattern != MovementPattern.FollowWalls)
        {
            myTarget.movementTarget = (Transform)EditorGUILayout.ObjectField("Target", myTarget.movementTarget, typeof(Transform), true);
        }
        else if (myTarget.movementPattern == MovementPattern.Random)
        {
            myTarget.frequency = EditorGUILayout.FloatField("Frequency", myTarget.frequency);
        }


        myTarget.movementType = (MovementType)EditorGUILayout.EnumPopup("Movement TYpe", myTarget.movementType);

    }
}
