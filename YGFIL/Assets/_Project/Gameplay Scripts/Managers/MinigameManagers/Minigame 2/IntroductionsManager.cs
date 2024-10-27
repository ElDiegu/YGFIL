using System.Collections;
using System.Collections.Generic;
using Systems.EventSystem;
using UnityEngine;
using YGFIL.Enums;
using YGFIL.Events;
using YGFIL.Managers;
using YGFIL.Minigames.PhaseTwo;
using YGFIL.ScriptableObjects;
using YGFIL.Systems;

namespace YGFIL.Minigames.Managers
{
    public class IntroductionsManager : StaticInstance<IntroductionsManager>, ISOContainer
    {
        [SerializeField] private IntroductionsOptionSetSO optionSet;
        public ScriptableObject ScriptableObject { get => optionSet; set {} }
        
        [SerializeField] private List<IntroductionsOptionSO> selectedOptions;
        [SerializeField] private GameObject submitButton;
        [SerializeField] private List<IntroductionsOptionCard> optionCards;
        
        private void Start() => optionSet = ((MonsterSO)DateManager.Instance.Monster.ScriptableObject).IntroductionsOptionSet;
        
#region Minigame State Machine
        public MinigameState state;
        
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
                    StartCoroutine(EndingCoroutine());
                    break;
                case MinigameState.Finished:
                    StartCoroutine(FinishedCoroutine());
                    break;
            }
            
        }
        
        private IEnumerator IntroductionCoroutine() 
        {
            DialogManager.Instance.PlayDialog(DialogTag.Introductions_Start.ToString());
            
            while (DialogManager.Instance.dialogState) yield return null;
            
            //DateManager.Instance.ActivateMinigame(1);
            
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent() 
            {
                animationString = "ShowIntroductionsMinigame"
            });
            
            yield return new WaitForSeconds(0.001f);
            
            while (UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            foreach(IntroductionsOptionCard optionCard in optionCards) 
            {
                optionCard.Slide();
                while (optionCard.IsSliding()) yield return null;
            }
            
            ChangeState(MinigameState.Game);
        }
        
        private IEnumerator EndingCoroutine() 
        {
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent() 
            {
                animationString = "HideIntroductionsMinigame"
            });
            
            yield return new WaitForSeconds(0.001f);
            
            while (UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            var totalLoveValue = 0;
            
            foreach (IntroductionsOptionSO introduction in selectedOptions) totalLoveValue += introduction.LoveValue;
            
            DialogManager.Instance.PlayDialog(totalLoveValue > 0 ? DialogTag.Introductions_Good.ToString() : DialogTag.Introductions_Bad.ToString());
            
            while (DialogManager.Instance.dialogState) yield return null;
            
            ChangeState(MinigameState.Finished);
        }
        
        public IEnumerator FinishedCoroutine() 
        {
            var totalLoveValue = 0;
            
            foreach (IntroductionsOptionSO introduction in selectedOptions) totalLoveValue += introduction.LoveValue;
            
            EventBus<UpdateLoveValueEvent>.Raise(new UpdateLoveValueEvent() 
            {
                loveValue = totalLoveValue
            });
            
            yield return null;
            
            DateManager.Instance.ChangePhase(DatePhase.MinigameThree);
        }
#endregion
        
        public void ChangeOptionStatus(IntroductionsOptionSO introductionOption, bool selected) 
        {
            if (selected && !selectedOptions.Contains(introductionOption)) selectedOptions.Add(introductionOption);
            else if (!selected && selectedOptions.Contains(introductionOption)) selectedOptions.Remove(introductionOption);
            
            if (selectedOptions.Count >= 3) submitButton.SetActive(true);
        }
        
        public void SubmitIntroduction() 
        {
            ChangeState(MinigameState.Ending);
            return;
        }
    }
}
