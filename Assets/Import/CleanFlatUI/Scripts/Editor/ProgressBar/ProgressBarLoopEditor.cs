
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditorInternal;

namespace RainbowArt.CleanFlatUI
{
    [CustomEditor(typeof(ProgressBarLoop))]
    public class ProgressBarLoop : Editor
    {
        SerializedProperty hasBackground;
        SerializedProperty background;
        SerializedProperty animator;

        protected virtual void OnEnable()
        {
            hasBackground = serializedObject.FindProperty("hasBackground");
            background = serializedObject.FindProperty("background");
            animator = serializedObject.FindProperty("animator");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();          
            EditorGUILayout.PropertyField(hasBackground); 
            if(hasBackground.boolValue == true)
            {
                EditorGUILayout.PropertyField(background);
            }           
            EditorGUILayout.PropertyField(animator);
            serializedObject.ApplyModifiedProperties();           
        }
    }
}
