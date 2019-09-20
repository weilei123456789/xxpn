using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;


namespace Penny
{

    public sealed class ModelTermEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(ModelTermEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }
     
        public List<bool> _Terms {
            get;
            private set;
        }

        public List<bool> Terms {
            get {
                return _Terms;
            }
        }

        public override void Clear()
        {
            _Terms = default(List<bool>);           
        }

        public ModelTermEventArgs(List<bool> tes) {
            _Terms = tes;
        }

    }
}