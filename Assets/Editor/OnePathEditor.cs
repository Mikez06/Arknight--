using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OnePath))]
public class OnePathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        OnePath inputPanel = (OnePath)target;
        if (GUILayout.Button("寻路预览"))
        {
            inputPanel.PreviewWayPoint();
        }
    }
}
