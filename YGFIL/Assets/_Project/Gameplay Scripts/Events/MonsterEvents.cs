using Systems.EventSystem;

namespace YGFIL.Events
{
    public struct LoveValueUpdatedEvent : IEvent 
    {
        public float loveValue;
    }
    public struct UpdateLoveValueEvent : IEvent 
    {
        public float loveValue;
    }
}
