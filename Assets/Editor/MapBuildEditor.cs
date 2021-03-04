using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapManager))]
public class MapBuildEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MapManager inputPanel = (MapManager)target;
        if (GUILayout.Button("自动设置地图信息"))
        {
            inputPanel.AutoBuild();
        }
    }
}
