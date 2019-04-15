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

namespace NEEEU {
    public class LerpBetween : MonoBehaviour {
        public Transform _target;
        [Range(0, 1)]
        public float _lerp;
        Vector3 _origPosition;
        Quaternion _origRotation;
        Vector3 _origScale;

        void Start() {
            _origPosition = transform.position;
            _origRotation = transform.rotation;
            _origScale = transform.localScale;
        }

        void Update() {
            transform.position = Vector3.Lerp(_origPosition, _target.position, _lerp);
            transform.rotation = Quaternion.Lerp(_origRotation, _target.rotation, _lerp);
            transform.localScale = Vector3.Lerp(_origScale, _target.localScale, _lerp);
        }
    }
}