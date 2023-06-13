
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditorInternal;

namespace RainbowArt.CleanFlatUI
{
    [CustomEditor(typeof(Switch))]
    public class SwitchEditor : Editor
    {
        SerializedProperty isOn;        
        SerializedProperty animator;        
        SerializedProperty switchOn;
        SerializedProperty switchOff;

        protected virtual void OnEnable()
        {
            isOn = serializedObject.FindProperty("isOn");
            animator = serializedObject.FindProperty("animator");              
            switchOn = serializedObject.FindProperty("switchOn");
            switchOff = serializedObject.FindProperty("switchOff");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(isOn); 
            EditorGUILayout.PropertyField(animator); 
            EditorGUILayout.PropertyField(switchOn);
            EditorGUILayout.PropertyField(switchOff);   
            serializedObject.ApplyModifiedProperties();           
        }
    }
}
