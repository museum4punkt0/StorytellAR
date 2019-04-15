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
using UnityEngine.EventSystems;

namespace NEEEU {
    public class OSCEvent : MonoBehaviour, IPointerClickHandler {
#if UNITY_IOS
        public UnityEvent OnTrigger;

        public OscOut oscOut;

        public void Trigger() {
            var path = transform.GetPath();
            path = path.Replace(' ', '_');
            var message = new OscMessage(path);

            var id = System.Guid.NewGuid().ToString();
            message.Add(id);

            StartCoroutine(SendMessage(message));
        }

        public void OnPointerClick(PointerEventData data) {
            Trigger();
        }

        IEnumerator SendMessage(OscMessage message) {
            for (int i = 0; i < 10; i++) {
                oscOut.Send(message);
                yield return null;
            }
        }

        List<string> handledEvents = new List<string>();
        public void HandleEvent(string id) {
            if (handledEvents.Contains(id)) {
                return;
            }
            handledEvents.Add(id);
            OnTrigger.Invoke();
            Debug.Log("Triggered: " + id);
        }
#else
        public void OnPointerClick(PointerEventData data) {

        }
#endif
    }
}