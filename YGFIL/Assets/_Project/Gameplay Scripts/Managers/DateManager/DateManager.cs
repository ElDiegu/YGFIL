using System.Collections.Generic;
using Systems.EventSystem;
using UnityEditor;
using UnityEngine;
using YGFIL.Enums;
using YGFIL.Events;
using YGFIL.Minigames.Managers;
using YGFIL.Monsters;
using YGFIL.Systems;

namespace YGFIL.Managers
{
    public class DateManager : StaticInstance<DateManager>
    {
        [field: SerializeField]
        public DatePhase DatePhase { get; private set; }
        
        [SerializeField] private List<GameObject> minigameObjects;
        [SerializeField] private GameObject blockingImage;
        [field: SerializeField] public Monster Monster { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            
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
                    break;
            }
        }
        
        public System.Collections.IEnumerator IntroducctionCoroutine() 
        {
            Debug.Log("Starting Coroutine");
            
            EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent()
            {
                animationString = "FadeIn"
            });
            
            while (UIManager.Instance.UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            ChangePhase(DatePhase.MinigameOne);
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
