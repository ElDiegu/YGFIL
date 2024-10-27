using System.Collections;
using System.Collections.Generic;
using Systems.EventSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using YGFIL.Events;
using YGFIL.Managers;
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
        
        [Header("Images")]
        [SerializeField] private MonsterSO monsterSO;
        [SerializeField] private List<Sprite> heartSprite;
        [SerializeField] private Image heartImage;
        [SerializeField] private List<Sprite> borderSprite;
        [SerializeField] private Image borderImage;
        [SerializeField] private List<Sprite> fillSprite;
        [SerializeField] private Image fillImage;
        [SerializeField] private List<Sprite> backgroundSprite;
        [SerializeField] private Image backgroundImage;
        
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
        
        private IEnumerator Start() 
        {
            while (DateManager.Instance.Monster.ScriptableObject == null) yield return null;
            
            monsterSO = DateManager.Instance.Monster.ScriptableObject as MonsterSO;
            
            heartImage.sprite = heartSprite[(int)monsterSO.MonsterType - 2];
            borderImage.sprite = borderSprite[(int)monsterSO.MonsterType - 2];
            fillImage.sprite = fillSprite[(int)monsterSO.MonsterType - 2];
            backgroundImage.sprite = backgroundSprite[(int)monsterSO.MonsterType - 2];
        }
#endregion

        private void SetLoveThreshold()
        {
            var monsterSO = (MonsterSO)monster.GetComponent<Monster>().ScriptableObject;
            var loveThreshold = monsterSO.LoveThreshold;

            var position = loveThresholdTransform.parent.GetComponent<RectTransform>().rect.height * (loveThreshold / 100);

            loveThresholdTransform.anchoredPosition = new Vector3(0f, position, 0f);
            
            loveSlider.value = monsterSO.StartingLove;
        }

        private void UpdateLoveValue(LoveValueUpdatedEvent loveValueUpdatedEvent)
        {
            if (loveBarCoroutine != null)
            {
                StopCoroutine(loveBarCoroutine);
                Debug.Log("Stopping Coroutine");
            }
            
            if(loveValueUpdatedEvent.loveValue > loveSlider.value)
            {
                AudioManager.Instance.Play("loveBarUp");
            }
            else
            {
                AudioManager.Instance.Play("loveBarDown");
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
        
        public bool IsMoving() => loveBarCoroutine != null;
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
