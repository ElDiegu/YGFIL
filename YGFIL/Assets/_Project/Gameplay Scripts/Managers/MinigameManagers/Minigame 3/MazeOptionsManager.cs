using System.Collections.Generic;
using Systems.EventSystem;
using UnityEngine;
using YGFIL.Events;

namespace YGFIL
{
    public class MazeOptionsManager : MonoBehaviour
    {
        private int solvedMazes;
        [SerializeField] private List<GameObject> options;
        [SerializeField] private GameObject mazeZone, optionsZone;
        
        EventBinding<SolvedMazeEvent> solvedMazeEventBinding;
        
        private void OnEnable()
        {
            solvedMazeEventBinding = new EventBinding<SolvedMazeEvent>( _ => solvedMazes++ );
            EventBus<SolvedMazeEvent>.Register(solvedMazeEventBinding);
        }
        
        private void OnDisable() 
        {
            EventBus<SolvedMazeEvent>.Deregister(solvedMazeEventBinding);
        }

        private void ActivateOptions() 
        {
            for (int i = 0; i < solvedMazes; i++) 
            {
                options[i].SetActive(true);
            }
        }
    }
}
