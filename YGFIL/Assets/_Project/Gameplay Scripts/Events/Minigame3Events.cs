using Systems.EventSystem;
using UnityEngine;

namespace YGFIL.Events
{
    public struct GeneratedMazeEvent : IEvent 
    {
        public RectTransform startCell;
    }
    
    public struct SolvedMazeEvent : IEvent {}
    
    public struct FinishedMazePhase : IEvent {}
}
