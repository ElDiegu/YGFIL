using EventSystem;
using UnityEngine;

namespace YGFIL
{
    public struct LoveValueUpdatedEvent : IEvent 
    {
        public float loveValue;
    }
}
