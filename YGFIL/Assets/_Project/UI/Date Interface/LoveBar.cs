using System;
using System.Collections;
using Systems.EventSystem;
using UnityEngine;
using UnityEngine.UI;
using YGFIL.Events;
using YGFIL.Monsters;
using YGFIL.ScriptableObjects;

namespace YGFIL
{
    public class LoveBar : MonoBehaviour
    {
        [SerializeField] private GameObject monster;
        [SerializeField] private Slider backSlider, frontSlider;
        [SerializeField] private RectTransform loveThresholdTransform;
        [SerializeField] private float loveUpdateSpeed, minLoveUpdateMultiplier;
        private Coroutine currentCoroutine = null;

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

            var position = loveThresholdTransform.parent.GetComponent<RectTransform>().rect.height * (loveThreshold / 100);

            Debug.Log(loveThreshold / 100 + " " + position);

            loveThresholdTransform.anchoredPosition = new Vector3(0f, 480, 0f);
        }

        private void UpdateLoveValue(LoveValueUpdatedEvent loveValueUpdatedEvent)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                Debug.Log("Stopping Coroutine");
            }
            
            if (frontSlider.value < loveValueUpdatedEvent.loveValue) currentCoroutine = StartCoroutine(IncreaseLoveValueCoroutine(loveValueUpdatedEvent.loveValue));
            else currentCoroutine = StartCoroutine(DecreaseLoveValueCoroutine(loveValueUpdatedEvent.loveValue));
        }

        private IEnumerator IncreaseLoveValueCoroutine(float targetValue)
        {
            while (frontSlider.value != targetValue)
            {
                var multiplier = Mathf.Max(0.1f, Mathf.Abs(frontSlider.value - targetValue) * 0.1f);

                frontSlider.value = Mathf.MoveTowards(frontSlider.value, targetValue, loveUpdateSpeed * multiplier * Time.deltaTime);

                Debug.Log(multiplier);

                yield return null;
            }
            
            backSlider.value = targetValue;
            
            yield return null;
        }
        
        private IEnumerator DecreaseLoveValueCoroutine(float targetValue) 
        {
            frontSlider.value = targetValue;
            
            yield return new WaitForSeconds(0.5f);
            
            while (backSlider.value != targetValue)
            {
                var multiplier = Mathf.Max(0.1f, Mathf.Abs(backSlider.value - targetValue) * 0.1f);

                backSlider.value = Mathf.MoveTowards(backSlider.value, targetValue, loveUpdateSpeed * multiplier * Time.deltaTime);

                Debug.Log(multiplier);

                yield return null;
            }
            
            yield return null;
        }
    }
}
