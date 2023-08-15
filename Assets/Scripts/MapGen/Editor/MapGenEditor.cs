using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CherryTeaGames.Generators
{
    [CustomEditor(typeof(MapGen), editorForChildClasses: true)]
    public class MapGenEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // GUI.enabled = true;
            MapGen e = target as MapGen;
            if (GUILayout.Button("Generate Map")) e.GenerateMap();
            if (GUILayout.Button("Clear Map")) e.ResetBlocks();
        }
    }
}
