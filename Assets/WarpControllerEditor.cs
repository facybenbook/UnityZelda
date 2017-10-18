////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;
////using UnityEditor;

////[CustomEditor(typeof(WarpController))]
////public class WarpControllerEditor : Editor
////{
////    WarpController mytarget;

////    public void OnEnable()
////    {
////        mytarget = target as WarpController;
////    }

////    public override void OnInspectorGUI()
////    {
////        string[] options = new string[GameController.control.warps.Length];
////        for (int i = 0; i < options.Length; i++)
////            options[i] = GameController.control.warps[i].name;
////        int selectedIndex = 0;
////        mytarget.destinationWarp = GameController.control.warps[EditorGUILayout.Popup(selectedIndex, options)];
////    }
////}
