using System.Collections;
using Systems.EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YGFIL.Enums;
using YGFIL.Events;
using YGFIL.Managers;
using YGFIL.ScriptableObjects;
using YGFIL.Systems;

namespace YGFIL.Minigames.Managers
{
    public class Phase4Manager : StaticInstance<Phase4Manager>, ISOContainer
    {
        [SerializeField] private Phase4OptionSetSO optionSet;
        public ScriptableObject ScriptableObject { get => optionSet; set {} }

        [SerializeField] private Phase4OptionSO selectedOption;

        [SerializeField] private TMP_Text cardText;
        [SerializeField] private TMP_Text fillText;
        [SerializeField] private Image cardImage;
        [SerializeField] private TMP_Text[] optionButtonTexts;
        [SerializeField] private Image[] optionButtonImage;
        [SerializeField] private Button submitButton;
        int currentOption = -1;
        
        private IEnumerator Start()
        {
            while (DateManager.Instance.Monster.ScriptableObject == null) yield return null;
            
            optionSet = (DateManager.Instance.Monster.ScriptableObject as MonsterSO).Phase4optionSet;
            
            cardText.text = optionSet.CardText;
            cardImage.sprite = optionSet.CardImage;
            for (int i = 0; i < 3; i++)
            {
                optionButtonTexts[i].text = optionSet.Options[i].Text;
                optionButtonTexts[i].color = optionSet.Options[i].TextColor;
                optionButtonImage[i].sprite = optionSet.Options[i].Image;
            }
        }
        
        public void SubmitOption() 
        {
            selectedOption = optionSet.Options[currentOption];
            DateManager.Instance.EndTimerEarly();
            ChangeState(MinigameState.Ending);
        }
        
        public void OnOptionButtonPressed(int optionClicked)
        {
            currentOption = optionClicked;
            fillText.text = optionButtonTexts[optionClicked].text;
            submitButton.enabled = true;
        }
        
        public void TimeOut() 
        {
            currentOption = Random.Range(0, 3);
            SubmitOption();
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
            DialogManager.Instance.PlayDialog(DialogTag.Phase4_Start.ToString());
            
            while (DialogManager.Instance.dialogState) yield return null;
            
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent()
            {
                animationString = "ShowPhase4"
            });

            AudioManager.Instance.Play("napkin");

            while (UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            DateManager.Instance.StartMinigameTimer(15f);
            
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
