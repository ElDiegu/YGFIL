using System.Collections.Generic;
using System.Linq;
using Systems;
using Systems.EventSystem;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using YGFIL.Databases;
using YGFIL.Managers;
using YGFIL.Monsters;

namespace YGFIL.Systems
{
    public class DialogManager : StaticInstance<DialogManager>
    {
        [SerializeField] public List<Image> characterImages;
        [SerializeField] public List<GameObject> dialogUI;
        [SerializeField] private Monster monster;
        [SerializeField] private Typewriter typewriter;
        
        private List<DialogData> dialogList;
        private bool writing, endDialog;
        public bool dialogState;
        private int nextID = -1;
        
#region Events
        private EventBinding<OnTypweWriterFinishEvent> onTypewriterFinishBinding;
        private EventBinding<OnTypewriterStartEvent> onTypewriterStartBinding;
        
        private void OnEnable()
        {
            onTypewriterFinishBinding = new EventBinding<OnTypweWriterFinishEvent>( onTypeWriterFinishEvent => 
            {
                if (onTypeWriterFinishEvent.isEnd) endDialog = true;
                else nextID = onTypeWriterFinishEvent.nextID;
                writing = false;
            });
            EventBus<OnTypweWriterFinishEvent>.Register(onTypewriterFinishBinding);
            
            onTypewriterStartBinding = new EventBinding<OnTypewriterStartEvent>( _ => writing = true );
            EventBus<OnTypewriterStartEvent>.Register(onTypewriterStartBinding);
        }
        
        private void OnDisable()
        {
            EventBus<OnTypweWriterFinishEvent>.Deregister(onTypewriterFinishBinding);
            EventBus<OnTypewriterStartEvent>.Deregister(onTypewriterStartBinding);
        }
#endregion

        protected override void Awake()
        {
            base.Awake();
            
            LoadDialogData(monster.monsterType);
            
            characterImages[2].sprite = MonsterExpressionDatabase.GetSprite(monster.monsterType, MonsterExpressionType.Neutral);
            
            SetDialogUI(true);
        }
        
        private void Update()
        {
            var input = InputManager.Instance.GetInput();
            
            if (input.LeftClick && writing) 
            {
                typewriter.EndWriting();
                writing = false;
                return;
            }
            
            if(input.LeftClick && endDialog) EndDialog();
            else if(input.LeftClick && !endDialog) PlayDialog(nextID);
        }

        public void LoadDialogData(MonsterType monster)
        {
            dialogList = DialogImporter.ImportDialog("Dialogs - " + monster.ToString());
        }
        
        public void PlayDialog(string tag) 
        {
            var dialog = dialogList.Where( dialog => dialog.Tag == tag).FirstOrDefault();
            
            PlayDialog(dialogList.IndexOf(dialog));
        }
        
        public void PlayDialog(int dialogIndex) 
        {
            if (dialogIndex > dialogList.Count - 1 || writing) return;
            
            Debug.Log($"Playing Dialog with ID {dialogIndex}");
            
            SetDialogUI(true);
            
            DialogData dialog = dialogList[dialogIndex];
            
            SetExpression(dialog.Monster, dialog.Expression);
            
            typewriter.Write(dialog);
        }
        
        public void EndDialog() 
        {
            SetDialogUI(false);
        }
        
        public void SetExpression(MonsterType monsterType, MonsterExpressionType expression)
        {
            var index = Mathf.Clamp((int)monsterType, 0, 2);
            characterImages[index].sprite = MonsterExpressionDatabase.GetSprite(monsterType, expression);
        }
        
        public void SetDialogUI(bool state) 
        {
            //foreach (GameObject UIObject in dialogUI) UIObject.SetActive(state);
            dialogState = state;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DialogManager))]
    public class DialogManagerEditor : Editor
    {
        public static MonsterType monsterType;
        public static MonsterExpressionType expressionType;
        public static int dialogID = -1;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            monsterType = (MonsterType)EditorGUILayout.EnumFlagsField(monsterType);
            expressionType = (MonsterExpressionType)EditorGUILayout.EnumFlagsField(expressionType);

            if (GUILayout.Button("Change Sprite"))
            {
                var manager = (DialogManager)target;
                manager.SetExpression(monsterType, expressionType);
            }
            
            dialogID = EditorGUILayout.IntField(dialogID);
            
            if (GUILayout.Button("Play Dialog")) 
            {
                var manager = (DialogManager)target;
                manager.PlayDialog(dialogID);
            }
        }
    }
#endif
}

namespace YGFIL.Enums 
{
    public enum DialogTag 
    {
        End,
        IceBreaking_Start,
        IceBreaking_End
    }
}