using System.Collections.Generic;
using Systems.EventSystem;
using TMPro;
using UnityEngine;
using YGFIL.Enums;
using YGFIL.Events;
using YGFIL.ScriptableObjects;

namespace YGFIL
{
    public class BrainConnectionsManager : MonoBehaviour, ISOContainer
    {
        [SerializeField] private BrainConnectionsOptionSetSO brainConnectionsOptionSet;
        public ScriptableObject ScriptableObject { get => brainConnectionsOptionSet; set { } }
        
        private int solvedMazes;
        [SerializeField] private List<GameObject> options;
        [SerializeField] private GameObject mazeZone, optionsZone;
        
        [SerializeField] private MinigameState state;
        
        public void ChangeState() 
        {
            switch (state) 
            {
                case MinigameState.Introduction:

                    break;
                case MinigameState.Game:

                    break;
                case MinigameState.Ending:
                
                    break;
                case MinigameState.Finished:
                
                    break;
            }
        }
        
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
        
        private void Awake()
        {
            for (int i = 0; i < options.Count ; i++) options[i].GetComponentInChildren<TextMeshProUGUI>().text = brainConnectionsOptionSet.Options[i].Text;
        }

        private void ActivateOptions() 
        {
            for (int i = 0; i < solvedMazes; i++) 
            {
                options[i].SetActive(true);
            }
        }
        
        private void SubmitOption(int index) 
        {
            EventBus<UpdateLoveValueEvent>.Raise(new UpdateLoveValueEvent() 
            {
                loveValue = brainConnectionsOptionSet.Options[index].LoveValue,
            });
        }
    }
}
