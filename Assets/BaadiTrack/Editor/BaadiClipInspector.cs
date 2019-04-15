/*

Copyright © 2018 NEEEU Spaces GmbH

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace NEEEU.Baadi
{

    [CustomEditor(typeof(BaadiClip))]
    public class BaadiClipInspector : Editor
    {

        private SerializedProperty typeProperty;
        private SerializedProperty conditionProperty;

        private void OnEnable()
        {
            typeProperty = serializedObject.FindProperty("type");
            conditionProperty = serializedObject.FindProperty("condition");
        }

        public override void OnInspectorGUI()
        {
            bool isCue = false;

            EditorGUILayout.PropertyField(typeProperty);

            int index = typeProperty.enumValueIndex;
            BaadiBehaviour.BaadiType type = (BaadiBehaviour.BaadiType)index;

            switch (type)
            {
                case BaadiBehaviour.BaadiType.Cue:
                    isCue = true;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("cueLabel"));
                    break;

                case BaadiBehaviour.BaadiType.JumpToCue:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("cueToJumpTo"));
                    break;

                case BaadiBehaviour.BaadiType.JumpToTime:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("timeToJumpTo"));
                    break;

                case BaadiBehaviour.BaadiType.TriggerExternal:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("triggerObject"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("eventTrigger"));
                    break;
            }

            if (!isCue)
            {
                EditorGUILayout.Space();

                index = conditionProperty.enumValueIndex;
                BaadiBehaviour.Condition conditionType = (BaadiBehaviour.Condition)index;

                switch (conditionType)
                {
                    case BaadiBehaviour.Condition.Always:
                        EditorGUILayout.PropertyField(conditionProperty);
                        EditorGUILayout.HelpBox("The action will execute at the beginning of the clip", MessageType.Warning);
                        break;

                    case BaadiBehaviour.Condition.Never:
                        EditorGUILayout.PropertyField(conditionProperty);
                        EditorGUILayout.HelpBox("The clip is disabled", MessageType.Warning);
                        break;

                    case BaadiBehaviour.Condition.Repeat:
                        EditorGUILayout.PropertyField(conditionProperty);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("repetitions"));
                        EditorGUILayout.HelpBox("The clip will execute in gameplay ''repetitions'' times", MessageType.Warning);
                        break;

                        // case BaadiBehaviour.Condition.Function:
                        // EditorGUILayout.HelpBox("To be implemented", MessageType.Warning);
                        // EditorGUILayout.PropertyField(conditionProperty);
                        // break;
                }
            }

            serializedObject.ApplyModifiedProperties();

        }

    }

}