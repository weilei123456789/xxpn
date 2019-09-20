using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;

namespace Penny
{
    public enum NormalDifficulty {
        Easy = 1,
        Normal = 2,
        Hard = 3,
    }

    public class NormalDifficultyEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(NormalDifficultyEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }


        public int _Difficulty
        {
            get;
            private set;
        }

        public NormalDifficulty Difficulty
        {
            get
            {
                return (NormalDifficulty)_Difficulty;
            }
        }

        public override void Clear() {
            _Difficulty = default(int);
        }

        public NormalDifficultyEventArgs(int _DifNum) {
            _Difficulty = _DifNum;
        }
    }
}