using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CherryTeaGames.Core.Events
{
    [CustomEditor(typeof(GameEventListener), editorForChildClasses: true)]
    public class EventListenerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;
            GameEventListener e = target as GameEventListener;
            if (e.Event != null)
            {
                if (GUILayout.Button("Raise")) e.Response.Invoke();
            }
        }
    }
}
