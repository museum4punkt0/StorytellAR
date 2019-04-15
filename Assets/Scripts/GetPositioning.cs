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
using UnityEngine.UI;

namespace NEEEU {
    public class GetPositioning : MonoBehaviour {
        public Transform _transform;
        public Text _x;
        public Text _y;
        public Text _z;
        public Text _a;
        public Text _b;
        public Text _g;

        void Update() {
            var pos = _transform.localPosition;
            _x.text = "x:" + (pos.x).ToString("F2");
            _y.text = "y:" + (pos.y).ToString("F2");
            _z.text = "z:" + (pos.z).ToString("F2");
            var rot = _transform.localEulerAngles;
            _a.text = "a:" + (rot.x).ToString("F2");
            _b.text = "b:" + (rot.y).ToString("F2");
            _g.text = "g:" + (rot.z).ToString("F2");
        }
    }
}