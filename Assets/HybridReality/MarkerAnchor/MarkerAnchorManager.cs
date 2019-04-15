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
using UnityEngine.XR.iOS;

namespace NEEEU.HybridReality {

    public class MarkerAnchorManager : MonoBehaviour {

        public ARReferenceImagesSet anchorImagesSet;

        [Header("Debug Options")]
        //public bool enableDebug = true;
        public bool enableInst = true;
        public GameObject prefabToInstantiate;
        public static MarkerAnchorManager manager;
        public static List<MarkerAnchor> mList = new List<MarkerAnchor>();

        void Start() {
            if (manager != null) {
                //Debug.LogError("Double marker managers");
                Object.Destroy(this);
                return;
            }

            manager = this;

            UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AnchorAdded;
        }


        void AnchorAdded(ARImageAnchor arImageAnchor) {
            //Debug.Log("Marker detected:"+arImageAnchor.referenceImageName);
            foreach (MarkerAnchor mInst in mList) {
                if (arImageAnchor.referenceImageName != mInst.referenceImage.imageName) {
                    //mInst.enableDebug = enableDebug;
                    continue;
                }
                mInst.AnchorAdded(arImageAnchor);
                //Debug.Log("anchor image: " + mInst.name + " in list.");
                return;
            }
            if (!enableInst) {
                return;
            }
            InstMarker(arImageAnchor);
            UnityARSessionNativeInterface.GetARSessionNativeInterface().RemoveUserAnchor(arImageAnchor.identifier);
        }

        void InstMarker(ARImageAnchor arImageAnchor) {
            Vector3 position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
            Quaternion rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);

            GameObject marker = Instantiate(prefabToInstantiate, position, rotation);


            marker.name = "Marker: " + arImageAnchor.referenceImageName;
            MarkerAnchor mInst = marker.GetComponent<MarkerAnchor>();
            mInst.referenceImageStr = arImageAnchor.referenceImageName;
            foreach (ARReferenceImage rInst in anchorImagesSet.referenceImages) {
                if (rInst.imageName != arImageAnchor.referenceImageName) {
                    continue;
                }
                mInst.referenceImage = rInst;
                break;
            }

        }


    }
}