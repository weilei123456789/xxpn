using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;

namespace Penny
{
    public class ModelPressEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ModelPressEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public bool _IsPress {
            get;
            private set;
        }

        public bool IsPress {
            get {
                return _IsPress;
            }
        }


        public override void Clear()
        {
            _IsPress = default(bool);
        }

        public ModelPressEventArgs(bool isp) {
            if (isp == IsPress)
            {
                return;
            }
            else
            {
                _IsPress = isp;
            }
        }


    }
}