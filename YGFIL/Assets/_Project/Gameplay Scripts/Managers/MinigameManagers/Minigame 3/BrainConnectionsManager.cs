using System;
using System.Collections;
using System.Collections.Generic;
using Systems.EventSystem;
using TMPro;
using UnityEngine;
using YGFIL.Enums;
using YGFIL.Events;
using YGFIL.Managers;
using YGFIL.Managers.Minigames;
using YGFIL.ScriptableObjects;
using YGFIL.Systems;

namespace YGFIL
{
    public class BrainConnectionsManager : StaticInstance<BrainConnectionsManager>, ISOContainer
    {
        [SerializeField] private BrainConnectionsOptionSetSO brainConnectionsOptionSet;
        public ScriptableObject ScriptableObject { get => brainConnectionsOptionSet; set { } }
        
        private int solvedMazes;
        [SerializeField] private List<GameObject> options;
        [SerializeField] private GameObject mazeZone, optionsZone;
        [SerializeField] private int selectedIndex;
        
        private void Start() => brainConnectionsOptionSet = (DateManager.Instance.Monster.ScriptableObject as MonsterSO).BrainConnectionsOptionSet;
        
        [SerializeField] private MinigameState state;
        public void ChangeState(MinigameState newState) 
        {
            state = newState;
            
            switch (state) 
            {
                case MinigameState.Introduction:
                    DateManager.Instance.SetInteraction(false);
                    StartCoroutine(IntroductionCoroutine());
                    break;
                case MinigameState.Game:
                    DateManager.Instance.SetInteraction(true);
                    MazeManager.Instance.GenerateMaze();
                    break;
                case MinigameState.Ending:
                    DateManager.Instance.SetInteraction(false);
                    StartCoroutine(EndingCoroutine());
                    break;
                case MinigameState.Finished:
                    StartCoroutine(FinishedCoroutine());
                    break;
            }
        }
        
        private IEnumerator IntroductionCoroutine() 
        {
            DialogManager.Instance.PlayDialog(DialogTag.Brain_Start.ToString());
            
            while (DialogManager.Instance.dialogState) yield return null;
            
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent() 
            {
                animationString = "ShowBrainMaze"
            });
            
            while (UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            ChangeState(MinigameState.Game);
        }
        
        private IEnumerator EndingCoroutine() 
        {
            DialogTag tag = DialogTag.Brain_One;
            
            switch (selectedIndex) 
            {
                case 0:
                    tag = DialogTag.Brain_One;
                    break;
                case 1:
                    tag = DialogTag.Brain_Two;
                    break;
                case 2:
                    tag = DialogTag.Brain_Three;
                    break;
                case 3:
                    tag = DialogTag.Brain_Four;
                    break;
            }
            
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent() 
            {
                animationString = "HideBrainOptions"
            });
            
            yield return new WaitForSeconds(0.001f);
            
            while (UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            DialogManager.Instance.PlayDialog(tag.ToString());
            
            while(DialogManager.Instance.dialogState) yield return null;
            
            ChangeState(MinigameState.Finished);
        }
        
        private IEnumerator FinishedCoroutine() 
        {
            EventBus<UpdateLoveValueEvent>.Raise(new UpdateLoveValueEvent() 
            {
                loveValue = brainConnectionsOptionSet.Options[selectedIndex].LoveValue,
            });
            
            yield return null;
            
            DateManager.Instance.ChangePhase(DatePhase.MinigameFour);
        }
        
        private IEnumerator ShowOptionsCoroutine() 
        {
            DateManager.Instance.SetInteraction(false);
            
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent() 
            {
                animationString = "HideBrainMazeShowOptions"
            });
            
            while (UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            DateManager.Instance.SetInteraction(true);
        }
        
        EventBinding<SolvedMazeEvent> solvedMazeEventBinding;

        private void OnEnable()
        {
            solvedMazeEventBinding = new EventBinding<SolvedMazeEvent>( _ => 
            {
                solvedMazes++;
                if (solvedMazes >= 4) StartCoroutine(ShowOptionsCoroutine());
            });
            EventBus<SolvedMazeEvent>.Register(solvedMazeEventBinding);
        }
        
        private void OnDisable() 
        {
            EventBus<SolvedMazeEvent>.Deregister(solvedMazeEventBinding);
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            brainConnectionsOptionSet = ((MonsterSO)DateManager.Instance.Monster.ScriptableObject).BrainConnectionsOptionSet;
            
            for (int i = 0; i < options.Count ; i++) options[i].GetComponentInChildren<TextMeshProUGUI>().text = brainConnectionsOptionSet.Options[i].Text;
        }

        private void ActivateOptions() 
        {
            for (int i = 0; i < solvedMazes; i++) 
            {
                options[i].SetActive(true);
            }
        }
        
        public void SubmitOption(int index) 
        {
            selectedIndex = index;
            
            ChangeState(MinigameState.Ending);
        }
    }
}
