using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Systems.EventSystem;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using YGFIL.Databases;
using YGFIL.Managers;
using YGFIL.Monsters;
using YGFIL.ScriptableObjects;

namespace YGFIL.Systems
{
    public class DialogManager : StaticInstance<DialogManager>
    {
        [SerializeField] public List<Image> characterImages;
        [SerializeField] public List<Image> characterBackground;
        [SerializeField] public List<GameObject> dialogUI;
        [SerializeField] private MonsterSO monsterSO;
        [SerializeField] private Typewriter typewriter;
        [SerializeField] private TextMeshProUGUI nameText, dialogText, casterText;
        [SerializeField] private GameObject characterNameObject, monsterNameObject, dialogBackground;
        
        private List<DialogData> dialogList;
        [SerializeField] private bool writing, endDialog;
        [SerializeField] public bool dialogState;
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
        
        private IEnumerator Start() 
        {
            while (DateManager.Instance.Monster.ScriptableObject == null) yield return null;
            
            monsterSO = DateManager.Instance.Monster.ScriptableObject as MonsterSO;
            
            LoadDialogData(monsterSO.MonsterType);
            
            characterImages[2].sprite = MonsterExpressionDatabase.GetSprite(monsterSO.MonsterType, MonsterExpressionType.Neutral);
            
            monsterNameObject.GetComponent<TextMeshProUGUI>().text = monsterSO.Name;
            
            SetDialogUI(true);
        }
        
        private void Update()
        {
            if (!dialogState) return;
            
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
            var dialog = dialogList.Where( dialog => dialog.Tag.Contains (tag)).FirstOrDefault();
            
            PlayDialog(dialogList.IndexOf(dialog));
        }
        
        public void PlayDialog(int dialogIndex) 
        {
            if (dialogIndex > dialogList.Count - 1 || writing) return;
            
            endDialog = false;
            dialogState = true;
            
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
            
            switch (monsterType) 
            {
                case MonsterType.MainCharacter:
                    characterBackground[0].sprite = ImagesDatabase.DialogSprites[0];
                    characterBackground[1].sprite = ImagesDatabase.DialogSprites[1];
                    characterNameObject.SetActive(true);
                    monsterNameObject.SetActive(false);
                    dialogBackground.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    break;
                case MonsterType.Caster:
                    characterBackground[0].sprite = ImagesDatabase.DialogSprites[1];
                    characterBackground[1].sprite = ImagesDatabase.DialogSprites[1];
                    break;
                default:
                    characterBackground[0].sprite = ImagesDatabase.DialogSprites[1];
                    characterBackground[1].sprite = ImagesDatabase.DialogSprites[0];
                    characterNameObject.SetActive(false);
                    monsterNameObject.SetActive(true);
                    dialogBackground.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    break;
            }
        }
        
        public void SetDialogUI(bool state) 
        {
            //foreach (GameObject UIObject in dialogUI) UIObject.SetActive(state);
            dialogState = state;
        }
        
        public void ClearDialog() 
        {
            nameText.text = "";
            dialogText.text = "";
            casterText.text = "";
            
            characterBackground[0].sprite = ImagesDatabase.DialogSprites[2];
            characterBackground[1].sprite = ImagesDatabase.DialogSprites[2];
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

