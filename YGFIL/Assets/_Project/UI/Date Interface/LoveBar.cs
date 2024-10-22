using System;
using System.Collections;
using Systems.EventSystem;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UI;
using YGFIL.Events;
using YGFIL.Monsters;
using YGFIL.ScriptableObjects;
using YGFIL.Utils;

namespace YGFIL
{
    public class LoveBar : MonoBehaviour
    {
        [SerializeField] private GameObject monster;
        
        [Header("Love Bar Properties")]
        [SerializeField] private Slider loveSlider;
        [SerializeField] private RectTransform loveThresholdTransform;
        [SerializeField] private float loveUpdateSpeed, minLoveUpdateMultiplier;
        
        [Header("Heart Properties")]
        [SerializeField] private Transform heartTransform;
        [SerializeField] private float heartScalingFactor, heartBeatingSpeed, heartBeatingSpeedIncrement;
        private bool fastBeating;
        
        [Header("Animations")]
        [SerializeField] private Animator animator;
        
        private Coroutine loveBarCoroutine = null;
        private Coroutine heartCoroutine = null;

#region Events
        private EventBinding<LoveValueUpdatedEvent> loveValueUpdatedBinding;
        //private EventBinding<OnStartingMinigameEvent> onStartingMinigameBinding;
#endregion

#region Unity Methods        
        private void OnEnable()
        {
            loveValueUpdatedBinding = new EventBinding<LoveValueUpdatedEvent>(UpdateLoveValue);
            //onStartingMinigameBinding = new EventBinding<OnStartingMinigameEvent>( _ => ChangeHeartState(false));
            EventBus<LoveValueUpdatedEvent>.Register(loveValueUpdatedBinding);
            //EventBus<OnStartingMinigameEvent>.Register(onStartingMinigameBinding);
        }

        private void OnDisable()
        {
            EventBus<LoveValueUpdatedEvent>.Deregister(loveValueUpdatedBinding);
        }

        private void Awake()
        {
            SetLoveThreshold();
            
            BeatingHeart(false);
        }
#endregion

        private void SetLoveThreshold()
        {
            var monsterSO = (MonsterSO)monster.GetComponent<Monster>().ScriptableObject;
            var loveThreshold = monsterSO.LoveThreshold;

            var position = loveThresholdTransform.parent.GetComponent<RectTransform>().rect.height * (loveThreshold / 100);

            Debug.Log(loveThreshold / 100 + " " + position);

            loveThresholdTransform.anchoredPosition = new Vector3(0f, position, 0f);
        }

        private void UpdateLoveValue(LoveValueUpdatedEvent loveValueUpdatedEvent)
        {
            if (loveBarCoroutine != null)
            {
                StopCoroutine(loveBarCoroutine);
                Debug.Log("Stopping Coroutine");
            }
            
            loveBarCoroutine = StartCoroutine(UpdateLoveValueCoroutine(loveValueUpdatedEvent.loveValue));
            
            var monsterSO = (MonsterSO)monster.GetComponent<Monster>().ScriptableObject;
            
            if (fastBeating != loveValueUpdatedEvent.loveValue >= monsterSO.LoveThreshold)
            {
                fastBeating = loveValueUpdatedEvent.loveValue >= monsterSO.LoveThreshold;
                BeatingHeart(fastBeating);
            }
        }
        
        private IEnumerator UpdateLoveValueCoroutine(float targetValue) 
        {
            while (loveSlider.value != targetValue) 
            {
                var multiplier = Mathf.Max(0.1f, Mathf.Abs(loveSlider.value - targetValue) * 0.1f);

                loveSlider.value = Mathf.MoveTowards(loveSlider.value, targetValue, loveUpdateSpeed * multiplier * Time.deltaTime);

                yield return null;
            }
        }

        private void BeatingHeart(bool fastBeating) 
        {
            if (heartCoroutine != null) StopCoroutine(heartCoroutine);
            
            var maxScale = 1 + heartScalingFactor * (fastBeating ? 1.5f : 1f);
            var minScale = 1 - heartScalingFactor * (fastBeating ? 1.5f : 1f);
            var speed = fastBeating ? heartBeatingSpeed + heartBeatingSpeedIncrement : heartBeatingSpeed;
            
            heartCoroutine = StartCoroutine(ShakeUtils.BeatingImageCoroutine(heartTransform, maxScale, minScale, speed));
        }
        
        public void ChangeHeartState(bool show) 
        {
            animator.Play(show ? "ShowLoveBarAnimation" : "HideLoveBarAnimation");
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(LoveBar))]
    public class LoveBarEditor : Editor 
    {
        public static bool loveBarState = true;
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Show Bar")) 
            {
                loveBarState = !loveBarState;
                ((LoveBar)target).ChangeHeartState(loveBarState);
            }
        }
    }
#endif
}
