using Systems.EventSystem;
using UnityEngine;

namespace YGFIL.Events
{
    public struct SubmitIceOptionEvent : IEvent
    {
        public int option;
        public float value;
    }
}
