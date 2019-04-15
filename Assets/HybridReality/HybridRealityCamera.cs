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
using NEEEU;
using NEEEU.HybridReality;

namespace NEEEU {
    [RequireComponent(typeof(Camera))]
    public class HybridRealityCamera : MonoBehaviour {

        Camera cam;
        private UnityARVideo aRVideo;
        public Material clearMaterial;
        private UnityARCameraNearFar camNear;
        private UnityARCameraManager cameraManager;
        [Header("AR Session Options")]
        public UnityARAlignment startAlignment = UnityARAlignment.UnityARAlignmentGravity;
        public UnityARPlaneDetection planeDetection = UnityARPlaneDetection.None;
        public bool getPointCloud = true;
        public bool enableLightEstimation = false;
        public bool enableAutoFocus = true;
        [Header("Markers Options")]
        public ARReferenceImagesSet detectionImages = null;
        private MarkerAnchorManager markerManager;
        //public bool enableDebug = true;
        public bool enableInstancing = true;
        public GameObject prefabToInstantiate;



        // Use this for initialization
        void Start() {

            cam = GetComponent<Camera>();
            cam.clearFlags = CameraClearFlags.Depth;
            cam.nearClipPlane = 0.01f;
            cam.depth = -1;
            cam.tag = "MainCamera";
            aRVideo = gameObject.AddComponent<UnityARVideo>();//  new UnityARVideo();
            if (clearMaterial == null) clearMaterial = new Material(Shader.Find("Unlit/ARCameraShader"));//
            aRVideo.m_ClearMaterial = clearMaterial;
            camNear = gameObject.AddComponent<UnityARCameraNearFar>();// new UnityARCameraNearFar();
            cameraManager = gameObject.AddComponent<UnityARCameraManager>();// new UnityARCameraManager();
            cameraManager.m_camera = cam;
            cameraManager.startAlignment = startAlignment;
            cameraManager.planeDetection = planeDetection;
            cameraManager.getPointCloud = getPointCloud;
            cameraManager.enableLightEstimation = enableLightEstimation;
            cameraManager.enableAutoFocus = enableAutoFocus;
            cameraManager.detectionImages = detectionImages;
            markerManager = gameObject.AddComponent<MarkerAnchorManager>();
            markerManager.anchorImagesSet = detectionImages;
            //markerManager.enableDebug = enableDebug;
            markerManager.enableInst = enableInstancing;
            markerManager.prefabToInstantiate = prefabToInstantiate;
        }

        // Update is called once per frame
        void Update() {

        }
    }

}