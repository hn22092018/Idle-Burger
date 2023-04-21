using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
//[CustomEditor(typeof(Test))]

//public class TestEditor : Editor {
//    public override void OnInspectorGUI() {
//        base.OnInspectorGUI();
//        Test test = (Test)target;
//        serializedObject.Update();
//        if (GUILayout.Button("Calculate!")) {
//            ClearLog();
//            EditorUtility.SetDirty((Test)target);
//            test.Calculate();
//        }

//        serializedObject.ApplyModifiedProperties();
//    }

//    public void ClearLog() //you can copy/paste this code to the bottom of your script
//    {
//        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
//        var type = assembly.GetType("UnityEditor.LogEntries");
//        var method = type.GetMethod("Clear");
//        method.Invoke(new object(), null);
//    }
//}
#endif