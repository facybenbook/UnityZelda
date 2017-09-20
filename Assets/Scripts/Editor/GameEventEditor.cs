using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[CustomEditor (typeof(GameEvent))]
public class GameEventEditor : Editor {
	public GameEvent myTarget;
	bool allTriggers;
	static int enumLength;
	SerializedProperty triggersProp;
	SerializedProperty onTriggerProp;
	SerializedProperty onUpdateProp;
	// Use this for initialization

	void Awake()
	{
		triggersProp = serializedObject.FindProperty ("triggersOn");
		onTriggerProp = serializedObject.FindProperty ("onTriggerCommands");
		onUpdateProp = serializedObject.FindProperty ("onUpdateCommands");

		myTarget = target as GameEvent;
		myTarget.Init ();
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField (triggersProp);
		EditorGUI.indentLevel++;
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PropertyField(triggersProp.GetArrayElementAtIndex(0), new GUIContent(GameEvent.activatorNames[0]));
		EditorGUILayout.PropertyField(triggersProp.GetArrayElementAtIndex(1), new GUIContent(GameEvent.activatorNames[1]));
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Space ();
		for (int i = 2; i < triggersProp.arraySize; i++) {
			if (i % 2 == 0)
				EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PropertyField(triggersProp.GetArrayElementAtIndex(i), new GUIContent(GameEvent.activatorNames[i]));
			if (i % 2 == 1 || i == triggersProp.arraySize - 1)
				EditorGUILayout.EndHorizontal ();
		}
		EditorGUI.indentLevel--;
		EditorGUILayout.PropertyField(onTriggerProp);
		EditorGUILayout.PropertyField(onUpdateProp);
		serializedObject.ApplyModifiedProperties ();
	}

}
