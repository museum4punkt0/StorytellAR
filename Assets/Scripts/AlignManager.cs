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

namespace NEEEU {
    public class AlignManager : MonoBehaviour {
        public Camera _camera;
        public LayerMask _hideLayers;
        public GameObject _ui;

        UnityARSessionNativeInterface session {
            get {
                if (_session == null) {
                    _session = UnityARSessionNativeInterface.GetARSessionNativeInterface();
                }
                return _session;
            }
        }
        UnityARSessionNativeInterface _session;
        ARTrackingState _trackingState;

        LayerMask _layers;
        void Start() {
            _layers = _camera.cullingMask;
            SetupAR();
            LoseAlignment();
        }
        void Update() {

        }
        public void LoseAlignment() {
            _camera.cullingMask = _hideLayers;
            _ui.SetActive(true);
        }
        public void GetAlignment() {
            _camera.cullingMask = _layers;
            _ui.SetActive(false);
        }
        void SetupAR() {
            UnityARSessionNativeInterface.ARFrameUpdatedEvent += TrackARState;
        }

        void ClearAR() {
            UnityARSessionNativeInterface.ARFrameUpdatedEvent -= TrackARState;
        }

        void TrackARState(UnityARCamera cam) {
            _trackingState = cam.trackingState;
            if (_trackingState == ARTrackingState.ARTrackingStateLimited || _trackingState == ARTrackingState.ARTrackingStateNotAvailable) {
                LoseAlignment();
            }
        }

        void OnDestroy() {
            ClearAR();
        }
    }
}