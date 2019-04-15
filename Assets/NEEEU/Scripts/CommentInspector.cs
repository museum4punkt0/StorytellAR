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
﻿/**
* Created by NEEEU Spaces GmbH.
* User: Stef Tervelde
*/
#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Comment))]
public class CommentInspector : Editor {
    public override void OnInspectorGUI() {
        // create a texture for the background
        Texture2D bgTex = new Texture2D(2, 2);
        bgTex.SetColor(new Color32(255, 251, 252, 255));

        // create a color for the text
        Color fontColor = new Color32(48, 51, 46, 255);

        Comment cmt = (Comment)target;
        if (editComments) {
            cmt.comment = EditorGUILayout.TextArea(cmt.comment, GUILayout.Height(200));
        }
        else {
            EditorGUILayout.LabelField(cmt.comment, new GUIStyle()
            {
                wordWrap = true,
                padding = new RectOffset()
                {
                    top = -2,
                    bottom = 15,
                    left = 0,
                    right = 5
                },
                overflow = new RectOffset()
                {
                    top = 15,
                    bottom = 2,
                    left = 12,
                    right = 5
                },
                fontSize = 12,
                richText = true,
                normal = new GUIStyleState() { textColor = fontColor, background = bgTex }
            });
        }
    }
    private static bool prefsLoaded = false;

    // The Preferences
    public static bool editComments = false;

    // Add preferences section named "Comment" to the Preferences Window
    [PreferenceItem("Comment")]

    public static void PreferencesGUI() {
        // Load the preferences
        if (!prefsLoaded) {
            editComments = EditorPrefs.GetBool("EditComents", false);
            prefsLoaded = true;
        }

        // Preferences GUI
        editComments = EditorGUILayout.Toggle("Edit Comments", editComments);

        // Save the preferences
        if (GUI.changed)
            EditorPrefs.SetBool("editComments", editComments);
    }
    [MenuItem("Edit/Toggle Edit Comments &c")]
    public static void ToggleCommentEdit() {
        editComments = !editComments;
        EditorPrefs.SetBool("editComments", editComments);
    }
}

#endif