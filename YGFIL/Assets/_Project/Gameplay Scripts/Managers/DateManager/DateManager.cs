using System.Collections;
using System.Collections.Generic;
using Systems.EventSystem;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using YGFIL.Enums;
using YGFIL.Minigames.Managers;
using YGFIL.Monsters;
using YGFIL.ScriptableObjects;
using YGFIL.Systems;
using YGFIL.Utils;

namespace YGFIL.Managers
{
    public class DateManager : StaticInstance<DateManager>
    {
        [field: SerializeField]
        public DatePhase DatePhase { get; private set; }
        
        [SerializeField] private List<GameObject> minigameObjects;
        [SerializeField] private GameObject blockingImage;
        [SerializeField] private TextMeshProUGUI timer;
        private Coroutine timerCoroutine = null;
        [field: SerializeField] public Monster Monster { get; private set; }
        
        protected void Start() 
        {
            ChangePhase(DatePhase.Introduction);
        }    

        public void ChangePhase(DatePhase newPhase) 
        {
            this.DatePhase = newPhase;
            
            switch (newPhase) 
            {
                case DatePhase.Introduction:
                    StartCoroutine(IntroducctionCoroutine());
                    break;
                case DatePhase.MinigameOne:
                    IceBreakingManager.Instance.ChangeState(MinigameState.Introduction);
                    break;
                case DatePhase.MinigameTwo:
                    IntroductionsManager.Instance.ChangeState(MinigameState.Introduction);
                    minigameObjects[0].SetActive(false);
                    break;
                case DatePhase.MinigameThree:
                    BrainConnectionsManager.Instance.ChangeState(MinigameState.Introduction);
                    minigameObjects[1].SetActive(false);
                    break;
                case DatePhase.MinigameFour:
                    Phase4Manager.Instance.ChangeState(MinigameState.Introduction);
                    minigameObjects[2].SetActive(false);
                    break;
                case DatePhase.MinigameFive:
                    AffinityTestManager.Instance.ChangeState(MinigameState.Introduction);
                    minigameObjects[3].SetActive(false);
                    break;
                case DatePhase.Ending:
                    SetInteraction(false);
                    minigameObjects[4].SetActive(false);
                    StartCoroutine(EndingCoroutine());
                    break;
            }
        }
        
        public IEnumerator IntroducctionCoroutine() 
        {   
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent()
            {
                animationString = "FadeIn"
            });
            
            while (UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            DialogManager.Instance.PlayDialog(DialogTag.Introduction.ToString());
            
            while (DialogManager.Instance.dialogState) yield return null;
            
            ChangePhase(DatePhase.MinigameOne);
        }
        
        private IEnumerator EndingCoroutine() 
        {
            DialogManager.Instance.PlayDialog(DialogTag.Ending.ToString());
            
            while(DialogManager.Instance.dialogState) yield return null;
            
            var result = Monster.loveValue >= (Monster.ScriptableObject as MonsterSO).LoveThreshold ? DialogTag.Ending_Like : DialogTag.Ending_Dislike;
            
            DialogManager.Instance.PlayDialog(result.ToString());
            
            while(DialogManager.Instance.dialogState) yield return null;
            
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent()
            {
                animationString = "FadeOut"
            });
            
            while (UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;

            if(Monster.loveValue >= (Monster.ScriptableObject as MonsterSO).LoveThreshold)
            {
                AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("EndDate");
            }
            else
            {
                AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("MonsterSelector");
            }
            
        }
        
        public void ActivateMinigame(int index) 
        {
            foreach (GameObject minigame in minigameObjects) minigame.SetActive(false);
            minigameObjects[index].SetActive(true);
        }
        
        public void SetInteraction(bool state) 
        {
            blockingImage.SetActive(!state);
        }
        
        public void StartMinigameTimer(float seconds) => timerCoroutine = StartCoroutine(TimerUtils.StartTimer(timer, seconds));
    
        public void TimeOut() 
        {
            timerCoroutine = null;
            
            switch (DatePhase) 
            {
                case DatePhase.MinigameOne:
                    IceBreakingManager.Instance.TimeOut();
                    break;
                case DatePhase.MinigameTwo:
                    IntroductionsManager.Instance.TimeOut();
                    break;
                case DatePhase.MinigameThree:
                    BrainConnectionsManager.Instance.TimeOut();
                    break;
                case DatePhase.MinigameFour:
                    Phase4Manager.Instance.TimeOut();
                    break;
                case DatePhase.MinigameFive:
                    AffinityTestManager.Instance.TimeOut();
                    break;
            }
        }
        
        public void EndTimerEarly() 
        {
            if (timerCoroutine != null) StopCoroutine(timerCoroutine);
            timerCoroutine = null;
            timer.text = "";
        }
        
        EventBinding<OnTimerFinishedEvent> onTimerFinishedBinding;
        
        private void OnEnable()
        {
            onTimerFinishedBinding = new EventBinding<OnTimerFinishedEvent>(TimeOut);
            
            EventBus<OnTimerFinishedEvent>.Register(onTimerFinishedBinding);
        }
        
        private void OnDisable()
        {
            EventBus<OnTimerFinishedEvent>.Deregister(onTimerFinishedBinding);
        }
    }
    
    public enum DatePhase 
    {
        Introduction,
        Conversation,
        MinigameOne,
        MinigameTwo,
        MinigameThree,
        MinigameFour,
        MinigameFive,
        Ending
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(DateManager))]
    public class DateManagerEditor : Editor
    {
        public static DatePhase datePhase = DatePhase.Introduction;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            DateManager dateManager = (DateManager)target;
            
            datePhase = (DatePhase)EditorGUILayout.EnumFlagsField(datePhase);
            
            if (GUILayout.Button("Change phase")) dateManager.ChangePhase(datePhase);
        }
    }
#endif
}
