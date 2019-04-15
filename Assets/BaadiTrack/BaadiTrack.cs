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
﻿using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace NEEEU.Baadi
{

    [TrackColor(0.0f, 0.5f, 1.0f)]
    [TrackClipType(typeof(BaadiClip))]
    public class BaadiTrack : TrackAsset
    {

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var scriptPlayable = ScriptPlayable<BaadiMixerBehaviour>.Create(graph, inputCount);
            BaadiMixerBehaviour mixer = scriptPlayable.GetBehaviour();
            mixer.cueClips = new System.Collections.Generic.Dictionary<string, double>();

            //This foreach will rename clips based on what they do, and collect the markers and put them into a dictionary
            //Since this happens when you enter Preview or Play mode, the object holding the Timeline must be enabled or you won't see any change in names
            //this is basically copied from the Timemachine example
            foreach (var c in GetClips())
            {
                BaadiClip clip = (BaadiClip)c.asset;
                string clipName = c.displayName;

                switch (clip.type)
                {
                    case BaadiBehaviour.BaadiType.Pause:
                        clipName = "||";
                        break;

                    case BaadiBehaviour.BaadiType.Cue:
                        clipName = "● " + clip.cueLabel.ToString();
                        if (!mixer.cueClips.ContainsKey(clip.cueLabel))
                        {
                            mixer.cueClips.Add(clip.cueLabel, (double)c.start);
                        }
                        break;

                    case BaadiBehaviour.BaadiType.JumpToCue:
                        clipName = "↩︎  " + clip.cueToJumpTo.ToString();
                        break;

                    case BaadiBehaviour.BaadiType.JumpToTime:
                        clipName = "↩︎  " + clip.timeToJumpTo.ToString();
                        break;

					case BaadiBehaviour.BaadiType.TriggerExternal:
                        clipName = "§  Trigger";
						break;

                }

                c.displayName = clipName;
            }

            return scriptPlayable;
        }

    }

}