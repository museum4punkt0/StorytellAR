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
using UnityEngine.Playables;
using UnityEngine.Timeline;
using NEEEU.Baadi;

namespace NEEEU {
    public class GoToCue : MonoBehaviour {
        PlayableDirector _director;
        void Start() {
            _director = GetComponent<PlayableDirector>();
            if (_director == null) {
                Destroy(this);
                Debug.Log("No Playable Director");
                return;
            }
        }

        public void Go(string cueName) {
            var tracks = _director.playableAsset as TimelineAsset;
            foreach (var track in tracks.GetOutputTracks()) {
                var cueTrack = track as BaadiTrack;
                if (cueTrack == null) {
                    continue;
                }
                foreach (var clips in cueTrack.GetClips()) {
                    var cue = clips.asset as BaadiClip;
                    if (cue == null || cue.cueLabel != cueName) {
                        continue;
                    }
                    _director.time = clips.start;
                    _director.Play();
                }
            }
        }
    }
}