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
    public class Phase4GameManager : StaticInstance<IceBreakingManager>, ISOContainer
    {
        [SerializeField] private Phase4OptionSetSO optionsSet;
        public ScriptableObject ScriptableObject { get => optionsSet; set {} }
        
        [SerializeField] private MinigameState state;

        [SerializeField] private Phase4OptionSO selectedOption;
        [SerializeField] private GameObject submitButton;
        
        public void ChangeSelectedOption(Phase4OptionSO option) 
        {
            submitButton.SetActive(true);
            selectedOption = option;
        }

        public void SubmitOption() 
        {
            ChangeState(MinigameState.Ending);
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
            DialogManager.Instance.PlayDialog(DialogTag.IceBreaking_Start.ToString());//Cambiar
            
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
            submitButton.SetActive(false);
            
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent()
            {
                animationString = "HidePhase4"
            });
            
            yield return new WaitForSeconds(0.001f);
            
            while(UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            DialogTag result = DialogTag.IceBreaking_Neutral;
            
            switch (selectedOption.LoveValue) 
            {
                case 5:
                    result = DialogTag.IceBreaking_Good;
                    break;
                case 3:
                    result = DialogTag.IceBreaking_Neutral;
                    break;
                case -2:
                    result = DialogTag.IceBreaking_Bad;
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
            
            DateManager.Instance.ChangePhase(DatePhase.MinigameTwo);
        }
    }
}
