using Systems.EventSystem;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using YGFIL.ScriptableObjects;
using YGFIL.Events;
using YGFIL.Managers;

namespace YGFIL.Monsters 
{
    public class Monster : MonoBehaviour, ISOContainer 
    {
        [SerializeField] private MonsterSO monsterSO;
        public ScriptableObject ScriptableObject { get => monsterSO; set {} }
        
        public float loveValue = 50f;
        public MonsterType monsterType;
        
        EventBinding<UpdateLoveValueEvent> updateLoveValueEventBinding;
        
        private void OnEnable()
        {
            updateLoveValueEventBinding = new EventBinding<UpdateLoveValueEvent>(UpdateLoveValue);
            EventBus<UpdateLoveValueEvent>.Register(updateLoveValueEventBinding);
        }
        
        private void OnDisable()
        {
            EventBus<UpdateLoveValueEvent>.Deregister(updateLoveValueEventBinding);
        }

        private void Awake()
        {
            if (GameManager.Instance != null) monsterSO = GameManager.Instance.Monster;
            monsterType = monsterSO.MonsterType;
        }
        
        private void Start() 
        {
            var monsterObject = Instantiate(monsterSO.Prefab, transform);
            monsterObject.transform.localPosition = Vector3.zero;
            monsterObject.transform.localScale = Vector3.one;
        }

        public void UpdateLoveValue(UpdateLoveValueEvent updateLoveValueEvent) 
        {
            loveValue += updateLoveValueEvent.loveValue;
            
            EventBus<LoveValueUpdatedEvent>.Raise(new LoveValueUpdatedEvent() 
            {
                loveValue = loveValue,
            });
        }
        
        public void SetLoveValue(float amount) 
        {
            loveValue = amount;
            
            EventBus<LoveValueUpdatedEvent>.Raise(new LoveValueUpdatedEvent() 
            {
                loveValue = loveValue,
            });
        }
    }
    
    public enum MonsterType
    {
        MainCharacter,
        Caster,
        Zombie,
        Spider,
        Medusa,
        Vampire,
        Succubus,
        Mummy,
        Wolf,
        Wolf2
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(Monster))]
    public class MonsterEditor : Editor 
    {
        static float loveValue = 0.0f;
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
            
            EditorGUILayout.Space();
            
            var monster = (Monster)target;
            
            loveValue = EditorGUILayout.Slider(loveValue, 0, 100);
            
            if (GUILayout.Button("Update Love Value")) 
            {
                monster.SetLoveValue(loveValue);
            }
        }
    }
#endif
}