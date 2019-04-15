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
﻿using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Events;

namespace NEEEU.Baadi
{
    [Serializable]
    public class BaadiClip : PlayableAsset, ITimelineClipAsset
    {
        [HideInInspector]
        public BaadiBehaviour template = new BaadiBehaviour();
        public BaadiBehaviour.BaadiType type;
        public BaadiBehaviour.Condition condition;
        public string cueToJumpTo = "";
        public string cueLabel = "";
        public float timeToJumpTo = 0f;

		public int repetitions = 1;

        public ExposedReference<GameObject> triggerObject;
        public UnityEvent eventTrigger;

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<BaadiBehaviour>.Create(graph, template);
            BaadiBehaviour instance = playable.GetBehaviour();
            instance.cueToJumpTo = cueToJumpTo;
            instance.type = type;
            instance.condition = condition;
            instance.cueLabel = cueLabel;
            instance.timeToJumpTo = timeToJumpTo;
			instance.repetitions = repetitions;

            return playable;
        }

    }
}
