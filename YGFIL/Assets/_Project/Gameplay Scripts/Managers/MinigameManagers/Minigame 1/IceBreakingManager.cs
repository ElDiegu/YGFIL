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
        [SerializeField] private IceOptionsSetSO setOptions;
        public ScriptableObject ScriptableObject { get => setOptions; set {} }
        
        private MinigameState state;

        [SerializeField] private IceOptionSO selectedOption;
        
        public void ChangeSelectedOption(IceOptionSO option) => selectedOption = option;

        public void SubmitOption() 
        {
            EventBus<UpdateLoveValueEvent>.Raise(new UpdateLoveValueEvent() 
            {
                loveValue = selectedOption.LoveValue
            });
        }
        
        public void ChangeState(MinigameState newState) 
        {
            state = newState;
            
            switch (state) 
            {
                case MinigameState.Introduction:
                    StartCoroutine(IntroductionCoroutine());
                    break;
                case MinigameState.Game:
                    DateManager.Instance.SetInteraction(true);
                    break;
            }
        }
        
        private IEnumerator IntroductionCoroutine() 
        {
            DateManager.Instance.SetInteraction(false);
            
            DialogManager.Instance.PlayDialog(DialogTag.IceBreaking_Start.ToString());
            
            while (DialogManager.Instance.dialogState) yield return null;
            
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent()
            {
                animationString = "ShowIceBreakingMinigame"
            });
            
            while(UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            ChangeState(MinigameState.Game);
            
            yield return null;
        }
    }
}
