using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BallsSetAsset
{

[CustomEditor(typeof(BallsSetConfig))]
public class BallsSetConfigEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        BallsSetConfig m_ballsSetConfig = (BallsSetConfig)target;

        if (GUILayout.Button("Set Balls Radius"))
        {
            m_ballsSetConfig.SetBallsRadius();
        }
    }
}

}