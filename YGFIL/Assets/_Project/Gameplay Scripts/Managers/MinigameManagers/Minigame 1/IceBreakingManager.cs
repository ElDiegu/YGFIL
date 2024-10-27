using System.Collections;
using Systems.EventSystem;
using UnityEngine;
using YGFIL.Enums;
using YGFIL.Events;
using YGFIL.Managers;
using YGFIL.ScriptableObjects;
using YGFIL.Systems;

namespace YGFIL.Minigames.Managers
{
    public class IceBreakingManager : StaticInstance<IceBreakingManager>, ISOContainer
    {
        [SerializeField] private IceOptionSetSO optionsSet;
        public ScriptableObject ScriptableObject { get => optionsSet; set {} }

        [SerializeField] public IceOptionSO selectedOption;
        [SerializeField] private GameObject submitButton;
        
        private void Start() => optionsSet = (DateManager.Instance.Monster.ScriptableObject as MonsterSO).IceBreakingOptionSet;

        public void ChangeSelectedOption(IceOptionSO option) 
        {
            submitButton.SetActive(true);
            selectedOption = option;
        }

        public void SubmitOption() 
        {
            DateManager.Instance.EndTimerEarly();
            ChangeState(MinigameState.Ending);
        }
        
        public void TimeOut() 
        {
            if (selectedOption == null) selectedOption = optionsSet.Options[Random.Range(0, optionsSet.Options.Count)];
            
            ChangeState(MinigameState.Ending);
        }
        
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
            DialogManager.Instance.PlayDialog(DialogTag.IceBreaking_Start.ToString());
            
            while (DialogManager.Instance.dialogState) yield return null;
            
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent()
            {
                animationString = "ShowIceBreakingMinigame"
            });
            
            while(UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            DateManager.Instance.StartMinigameTimer(15f);
            
            ChangeState(MinigameState.Game);
            
            yield return null;
        }
        
        private IEnumerator EndingCoroutine() 
        {
            submitButton.SetActive(false);
            
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent()
            {
                animationString = "HideIceBreakingMinigame"
            });
            
            yield return new WaitForSeconds(0.001f);
            
            while(UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            DialogTag result = DialogTag.IceBreaking_Neutral;
            
            if (selectedOption.LoveValue == 16) result = DialogTag.IceBreaking_Neutral;
            else if (selectedOption.LoveValue > 0) result = DialogTag.IceBreaking_Good;
            else if(selectedOption.LoveValue < 0) result = DialogTag.IceBreaking_Bad;
            else result = DialogTag.IceBreaking_Neutral;
            
            DialogManager.Instance.PlayDialog(result.ToString());
            
            while (DialogManager.Instance.dialogState) yield return null;
            
            ChangeState(MinigameState.Finished);
        }
        
        private IEnumerator FinishedCoroutine() 
        {
            EventBus<UpdateLoveValueEvent>.Raise(new UpdateLoveValueEvent() 
            {
                loveValue = selectedOption.LoveValue
            });
            
            yield return null;
            
            DateManager.Instance.ChangePhase(DatePhase.MinigameTwo);
        }
    }
}
