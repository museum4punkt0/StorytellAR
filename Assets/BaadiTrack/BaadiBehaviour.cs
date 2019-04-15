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

namespace NEEEU.Baadi
{
    public class BaadiBehaviour : PlayableBehaviour
    {

        public BaadiType type;
        public Condition condition;
        public string cueToJumpTo;
        public string cueLabel;
        public float timeToJumpTo;
		public int repetitions = 1;

        [HideInInspector]
        public bool clipExecuted = false; //this is for the mixer
        [HideInInspector]
        public int clipExecutedCount = 0;

        public bool ConditionMet()
        {
            switch (condition)
            {
                case Condition.Always:
                    return true;

                case Condition.Repeat:
                    if (clipExecutedCount < repetitions)
                    {
                        return true;
                    }
                    else
                    {
                        clipExecuted = true;
                        return false;
                    }

                case Condition.Never:
                default:
                    return false;
            }
        }



        public enum BaadiType
        {
            Cue,
            JumpToTime,
            JumpToCue,
            Pause,
            TriggerExternal,
        }

        public enum Condition
        {
            Always,
            Never,
            Repeat,
            //Function,
        }
    }

}


//ToDo
//1. add an external function condition (maybe just an external public boolean)