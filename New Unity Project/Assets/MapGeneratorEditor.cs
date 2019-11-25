using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Map_Generator))]
public class MapGeneratorEditor : Editor {

    public override void OnInspectorGUI()
    {
        Map_Generator mapGen = (Map_Generator)target;
        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }
        if (GUILayout.Button("Generator"))
        {
            mapGen.GenerateMap();
        }
    }

}
