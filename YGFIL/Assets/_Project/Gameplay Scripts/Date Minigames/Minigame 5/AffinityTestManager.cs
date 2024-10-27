using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YGFIL.Enums;
using System.Collections;
using YGFIL.Systems;
using Systems.EventSystem;
using YGFIL.Managers;
using System.Collections.Generic;
using YGFIL.ScriptableObjects;
using Unity.VisualScripting;
using YGFIL.Events;

namespace YGFIL
{
    public class AffinityTestManager : StaticInstance<AffinityTestManager>, ISOContainer
    {
        [SerializeField] private AffinityTestQuestionSetSO optionsSet;
        public ScriptableObject ScriptableObject { get => optionsSet; set {} }
        
        [SerializeField] private TMP_Text questionText;
        [SerializeField] private TMP_Text[] optionButtonTexts;
        
        [SerializeField] private List<int> selectedIndex;
        private int currentIndex = 0;
        private int currentLove = 0;

        public IEnumerator Start() 
        {
            while (DateManager.Instance.Monster.ScriptableObject == null) yield return null;
            
            optionsSet = (DateManager.Instance.Monster.ScriptableObject as MonsterSO).AffinityTestQuestionSetSO;
        }

        private void SetQuestion()
        {
            questionText.text = optionsSet.Questions[currentIndex].QuestionText;

            for (int i = 0; i < 4; i++)
            {
                optionButtonTexts[i].text = optionsSet.Questions[currentIndex].OptionsTexts[i];
            }
        }

        public void OnOptionButtonPressed(int optionClicked) 
        {
            DateManager.Instance.EndTimerEarly();
            
            selectedIndex.Add(optionClicked);
            currentLove += optionsSet.Questions[currentIndex].LoveValues[optionClicked];
            currentIndex++;
            
            if (currentIndex >= 3) ChangeState(MinigameState.Ending);
            else StartCoroutine(ShowNewQuestion());
        }
        
        public void TimeOut() 
        {
            OnOptionButtonPressed(Random.Range(0, 3));
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
            DialogManager.Instance.PlayDialog(DialogTag.Affinity_Start.ToString());
            
            while (DialogManager.Instance.dialogState) yield return null;
            
            SetQuestion();
            
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent()
            {
                animationString = "ShowAffinityTest"
            });
            
            while(UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            DateManager.Instance.StartMinigameTimer(30f);
            
            ChangeState(MinigameState.Game);
        }
        
        private IEnumerator EndingCoroutine() 
        {   
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent()
            {
                animationString = "HideAffinityTest"
            });
            
            yield return new WaitForSeconds(0.001f);
            
            while(UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            var tag = currentLove >= 0 ? DialogTag.Affinity_Good : DialogTag.Affinity_Bad;
            
            DialogManager.Instance.PlayDialog(tag.ToString());
            
            while(DialogManager.Instance.dialogState) yield return null;
            
            ChangeState(MinigameState.Finished);
        }
        
        private IEnumerator FinishedCoroutine() 
        {
            EventBus<UpdateLoveValueEvent>.Raise(new UpdateLoveValueEvent() 
            {
                loveValue = currentLove
            });
            
            yield return null;
            
            DateManager.Instance.ChangePhase(DatePhase.Ending);
        }
        
        private IEnumerator ShowNewQuestion() 
        {
            DateManager.Instance.SetInteraction(false);
            
            SetQuestion();
            
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent()
            {
                animationString = "ShowNewAffinityTestQuestion"
            });
            
            yield return new WaitForSeconds(0.001f);
            
            while(UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            DateManager.Instance.StartMinigameTimer(20f);
            
            DateManager.Instance.SetInteraction(true);
        }
    }
}
