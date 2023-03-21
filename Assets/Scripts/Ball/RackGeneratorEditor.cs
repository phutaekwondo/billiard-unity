using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(RackGenerator))]
public class RackGeneratorEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RackGenerator myScript = (RackGenerator)target;
        if(GUILayout.Button("Gen 9 balls"))
        {
            myScript.GenerateRack9Balls();
        }
        if(GUILayout.Button("Gen 15 balls"))
        {
            myScript.GenerateRack15Balls();
        }
        if(GUILayout.Button("Clear Rack"))
        {
            myScript.ClearRack();
        }
    }
}
