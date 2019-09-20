using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;

namespace Penny
{
    public class ModelLimitTimeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ModelLimitTimeEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public bool _IsOnTime {
            get;
            private set;
        }

        public bool IsOnTime
        {
            get {
                return _IsOnTime;
            }
        }


        public override void Clear()
        {
            _IsOnTime = default(bool);
        }

        public ModelLimitTimeEventArgs(bool isp) {
            if (isp == IsOnTime)
            {
                return;
            }
            else
            {
                _IsOnTime = isp;
            }
        }


    }
}