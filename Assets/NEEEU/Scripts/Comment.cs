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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class Comment : MonoBehaviour {

    [TextArea(1, 20)]

    public string comment;


    static Comment() {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;

    }

    private static Vector2 offset = new Vector2(0, 2);

    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect) {
        // create a texture for the background
        Texture2D bgTex = new Texture2D(2, 2);
        bgTex.SetColor(new Color32(255, 251, 252, 255));

        Color fontColor = new Color32(48, 51, 46, 255);

        //Color selColor = new Color32(98,187,193,255); // not sure how to set this, or if it is even possible

        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj == null) {
            return;
        }
        var gameObject = (GameObject)obj;
        if (gameObject.GetComponent<Comment>() == null) {
            return;
        }

        Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);

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
                normal = new GUIStyleState() { textColor = fontColor, background = bgTex }
            }
        );
    }
}

public static class Texture2DExtensions {

    public static void SetColor(this Texture2D tex2, Color32 color) {


        var fillColorArray = tex2.GetPixels32();

        for (var i = 0; i < fillColorArray.Length; ++i) {
            fillColorArray[i] = color;
        }

        tex2.SetPixels32(fillColorArray);

        tex2.Apply();
    }
}

#endif
