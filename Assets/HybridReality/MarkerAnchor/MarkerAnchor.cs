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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.iOS;

//Script to be used in any Image Anchor object you want to recognize
//the manager holds a list of markers and each object with this component will add itself to the list

namespace NEEEU.HybridReality {

    public enum AnchorType { WorldOrigin, Local }
    public class MarkerAnchor : MonoBehaviour {

        public AnchorType anchorType = AnchorType.Local;
        public ARReferenceImage referenceImage;
        public string referenceImageStr;
        public UnityEvent firstTimeDetected;
        public UnityEvent detected;
        private bool firstTime = true;

        //[Header("Debug Options")]
        //public bool enableDebug;
        //public Material Active;
        //public Material InActive;
        //public GameObject debugPrefab;

        float lastUpdate;
        //Vector4 referenceCuma;
        Vector3 referencePos;
        Quaternion referenceRot;
        Vector3 realWorldPos;
        Quaternion realWorldRot;

        public MarkerDebug debugger;


        void Start() {
            if (referenceImage == null && referenceImageStr == "") {
                return;
            }
            if (referenceImageStr == "") {
                referenceImageStr = referenceImage.imageName;
            }
            if (referenceImage == null) {
                if (MarkerAnchorManager.manager == null) {
                    Debug.LogError("No manager found!");
                    return;
                }
                foreach (ARReferenceImage rInst in MarkerAnchorManager.manager.anchorImagesSet.referenceImages) {
                    if (rInst.imageName != referenceImageStr) {
                        continue;
                    }
                    referenceImage = rInst;
                }
                if (referenceImage == null) {
                    Debug.LogError("Image not found!");
                    return;
                }
            }


            MarkerAnchorManager.mList.Add(this);
            referencePos = transform.position;
            referenceRot = transform.rotation;
        }


        void Update() {

        }
        void OnDestroy() {
            MarkerAnchorManager.mList.Remove(this);
        }

        public void AnchorAdded(ARImageAnchor arImageAnchor) {

            if (arImageAnchor.referenceImageName != referenceImageStr) {
                //Debug.Log("Not my image");
                return;
            }


            Vector3 position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
            Quaternion rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);

            if (debugger != null) {
                debugger.AnchorAdded(position, rotation);
            }

            if (anchorType == AnchorType.Local) {

                transform.position = position;
                transform.rotation = rotation;
                Debug.Log("Local anchor: " + arImageAnchor.referenceImageName);

            }
            else {

                GameObject originObject = new GameObject();
                originObject.transform.position = position;
                originObject.transform.rotation = rotation;

                UnityARSessionNativeInterface.GetARSessionNativeInterface().SetWorldOrigin(originObject.transform);
                Debug.Log("World anchor: " + arImageAnchor.referenceImageName + " at pos: " + position);

            }

            detected.Invoke();
            if (firstTime) {
                firstTimeDetected.Invoke();
                firstTime = false;
            }
            UnityARSessionNativeInterface.GetARSessionNativeInterface().RemoveUserAnchor(arImageAnchor.identifier);
        }

    }
}

