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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace NEEEU.Baadi
{
    public class BaadiMixerBehaviour : PlayableBehaviour
    {

        private PlayableDirector director;
        public Dictionary<string, double> cueClips;

        public override void OnPlayableCreate(Playable playable)
        {
            director = playable.GetGraph().GetResolver() as PlayableDirector;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (director.state == PlayState.Paused)
            {
                return;
            }

            int inputCount = playable.GetInputCount();
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                ScriptPlayable<BaadiBehaviour> inputPlayable = (ScriptPlayable<BaadiBehaviour>)playable.GetInput(i);
                BaadiBehaviour input = inputPlayable.GetBehaviour();

                if (inputWeight > 0f && !input.clipExecuted)
                {
                    switch (input.type)
                    {
                        case BaadiBehaviour.BaadiType.Pause:
                            if (input.ConditionMet())
                            {
                                director.Pause();
                            }
                            break;

                        case BaadiBehaviour.BaadiType.JumpToTime:
                            if (input.ConditionMet())
                            {
                                (playable.GetGraph().GetResolver() as PlayableDirector).time = (double)input.timeToJumpTo;
                            }
                            break;

                        case BaadiBehaviour.BaadiType.JumpToCue:
                            if (input.ConditionMet())
                            {
                                double t = cueClips[input.cueToJumpTo];
                                (playable.GetGraph().GetResolver() as PlayableDirector).time = t;
                            }
                            break;
                    }


                    input.clipExecuted = false;
                    input.clipExecutedCount++;
                    //input.clipExecutedCount %= 1000; //to not overflow, maybe????
                                                     //Debug.Log(input.clipExecutedCount);


                }
            }
        }

    }
}
