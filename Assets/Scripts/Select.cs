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
    [ExecuteInEditMode]
    public class Select : MonoBehaviour {
        public GameObject defaultActive;
        public List<GameObject> _options;
        GameObject _current;

        void Start() {
            if (!Application.isPlaying) {
                return;
            }
            foreach (var obj in _options) {
                obj.SetActive(false);
            }
            defaultActive.SetActive(true);
            _current = defaultActive;
        }
        void Update() {
            if (_options == null) return;
            foreach (var obj in _options) {
                if (obj == null) continue;
                if (obj.activeSelf && obj != _current) {
                    _current = obj;
                    foreach (var obj2 in _options) {
                        if (obj2 == obj) {
                            continue;
                        }
                        obj2.SetActive(false);
                    }
                }
            }
        }
    }
}