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
using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif

namespace NEEEU {
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class RequiredAttribute : System.Attribute {

    }
#if UNITY_EDITOR

    [InitializeOnLoad]
    class RequiredEditor {
        static RequiredEditor() {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
            EditorApplication.playModeStateChanged += HandlePlay;
        }
        private static void HandlePlay(PlayModeStateChange state) {
            MonoBehaviour[] monos = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();
            if (IsMissingAny(monos)) {
                EditorApplication.isPlaying = false;
            }
        }
        [PostProcessBuildAttribute(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
            MonoBehaviour[] monos = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();
            if (IsMissingAny(monos)) {
                Debug.LogError("Missing links!");
            }
        }
        private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect) {
            var obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj == null) {
                return;
            }
            var gameObject = (GameObject)obj;
            if (gameObject == null) {
                return;
            }
            MonoBehaviour[] monos = gameObject.GetComponents<MonoBehaviour>();

            if (!IsMissingAny(monos)) {
                return;
            }
            Color fontColor = new Color32(255, 0, 0, 255);
            // Debug.LogError("[" + mono.name + "]'s" + field.Name + "is null!");
            Rect offsetRect = new Rect(selectionRect.position + new Vector2(0, 2), selectionRect.size);

            EditorGUI.LabelField(
                offsetRect,
                obj.name,
                new GUIStyle()
                {
                    overflow = new RectOffset()
                    {
                        top = 2,
                        bottom = -2,
                        left = 20,
                        right = -10
                    },
                    normal = new GUIStyleState() { textColor = fontColor, }
                }
            );
        }
        private static bool IsMissingAny(MonoBehaviour[] monos) {
            foreach (MonoBehaviour mono in monos) {
                if (mono == null) continue;
                var type = mono.GetType();
                if (type == null) continue;
                FieldInfo[] objectFields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
                for (int i = 0; i < objectFields.Length; i++) {
                    var field = objectFields[i];
                    RequiredAttribute attribute = Attribute.GetCustomAttribute(field, typeof(RequiredAttribute)) as RequiredAttribute;
                    if (attribute == null) continue;
                    object value = field.GetValue(mono);
                    if (value == null) return true;
                }
            }
            return false;
        }
    }
#endif
}