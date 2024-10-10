using EventSystem;
using UnityEngine;
using UnityEngine.UI;
using YGFIL.Monsters;
using YGFIL.ScriptableObjects;

namespace YGFIL
{
    public class LoveBar : MonoBehaviour
    {
        [SerializeField] private GameObject monster;
        [SerializeField] private Slider loveSlider;
        [SerializeField] private RectTransform loveThresholdTransform;
        
#region Events
        private EventBinding<LoveValueUpdatedEvent> loveValueUpdatedBinding;
#endregion
        
#region Unity Methods        
        private void OnEnable()
        {
            loveValueUpdatedBinding = new EventBinding<LoveValueUpdatedEvent>(UpdateLoveValue);
            EventBus<LoveValueUpdatedEvent>.Register(loveValueUpdatedBinding);
        }
        
        private void OnDisable() 
        {
            EventBus<LoveValueUpdatedEvent>.Deregister(loveValueUpdatedBinding);
        }
        
        private void Awake()
        {
            SetLoveThreshold();
        }
#endregion
        
        private void SetLoveThreshold() 
        {
            var monsterSO = (MonsterSO)monster.GetComponent<Monster>().ScriptableObject;
            var loveThreshold = monsterSO.LoveThreshold;
            
            var position = loveSlider.GetComponent<RectTransform>().rect.height * (loveThreshold / 100);
            
            Debug.Log(loveThreshold / 100 + " " + position);
            
            loveThresholdTransform.anchoredPosition = new Vector3(0f, 480, 0f);
        }
        
        private void UpdateLoveValue(LoveValueUpdatedEvent loveValueUpdatedEvent) 
        {
            loveSlider.value = loveValueUpdatedEvent.loveValue;
        }
    }
}
