using System.Collections.Generic;
using Systems.EventSystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using YGFIL.Events;

namespace YGFIL.Managers
{
    public class DateManager : StaticInstance<DateManager>
    {
        [field: SerializeField]
        public DatePhase DatePhase { get; private set; }
        
        [SerializeField] private List<GameObject> minigameObjects;    

        public void ChangePhase(DatePhase newPhase) 
        {
            this.DatePhase = newPhase;
            
            switch (newPhase) 
            {
                case DatePhase.Introduction:

                case DatePhase.MinigameOne:
                    ActivateMinigame(0);
                    break;
            }
        }
        
        public void ActivateMinigame(int index) 
        {
            EventBus<OnStartingMinigameEvent>.Raise(new OnStartingMinigameEvent(){});
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
