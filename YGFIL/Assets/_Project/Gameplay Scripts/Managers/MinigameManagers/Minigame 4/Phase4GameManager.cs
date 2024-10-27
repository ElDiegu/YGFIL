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
    public class Phase4GameManager : StaticInstance<Phase4GameManager>, ISOContainer
    {
        [SerializeField] private Phase4OptionSetSO optionsSet;
        public ScriptableObject ScriptableObject { get => optionsSet; set {} }
        
        [SerializeField] private MinigameState state;

        [SerializeField] private Phase4OptionSO selectedOption;
        

        public void SubmitOption(int indexOption) 
        {
            ChangeState(MinigameState.Ending);
            selectedOption = optionsSet.Options[indexOption];
        }
        
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
            DialogManager.Instance.PlayDialog(DialogTag.Phase4_Start.ToString());
            
            while (DialogManager.Instance.dialogState) yield return null;
            
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent()
            {
                animationString = "ShowPhase4"
            });
            
            while(UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            ChangeState(MinigameState.Game);
            
            yield return null;
        }
        
        private IEnumerator EndingCoroutine() 
        {
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent()
            {
                animationString = "HidePhase4"
            });
            
            yield return new WaitForSeconds(0.001f);
            
            while(UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            DialogTag result = DialogTag.Phase4_Neutral;
            
            switch (selectedOption.LoveValue) 
            {
                case 5:
                    result = DialogTag.Phase4_Good;
                    break;
                case 3:
                    result = DialogTag.Phase4_Neutral;
                    break;
                case -2:
                    result = DialogTag.Phase4_Bad;
                    break;
            }
            
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
            
            DateManager.Instance.ChangePhase(DatePhase.MinigameFive);
        }
    }
}
