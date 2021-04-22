using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WayPointGenerateByDistanceUtil))]
public class WayPointGenerateByDistanceButton : Editor
{
    WayPointGenerateByDistanceUtil script;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        script = target as WayPointGenerateByDistanceUtil;
        if(GUILayout.Button("Generate WayPoint"))
        {
            script.WayPointGenerate();
        }
    }    
}

[CustomEditor(typeof(WayPointGenerator))]
public class WayPointGenerateButton : Editor
{
    WayPointGenerator script;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        script = target as WayPointGenerator;
        if (GUILayout.Button("Generate WayPoint"))
        {
            script.WayPointGenerate();
        }
    }
}

