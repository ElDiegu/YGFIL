using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor.UI;
using System.Collections.Generic;
using UnityEngine.UI;
using YGFIL.ScriptableObjects;

namespace YGFIL.Minigames.PhaseTwo
{
    public class OptionCard : MonoBehaviour, IDraggable, ISOContainer
    {
        [SerializeField] private IntroductionOptionSO optionSO;
        public ScriptableObject ScriptableObject { get => optionSO; set {} }
        
        [SerializeField] private Vector3 originalPosition;
        [SerializeField] private Transform originalParent;
        
        Physics physics;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            originalPosition = transform.position;
            originalParent = transform.parent;
            
            transform.SetParent(transform.root);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var mainCanvas = transform.root.GetComponent<Canvas>();
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.root as RectTransform,
                eventData.position,
                mainCanvas.worldCamera,
                out Vector2 position);
            
            transform.position = mainCanvas.transform.TransformPoint(position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            List<RaycastResult> hitResults = new List<RaycastResult>();
            
            var graphicRaycaster = transform.root.GetComponent<GraphicRaycaster>();
            graphicRaycaster.Raycast(eventData, hitResults);
            
            RaycastResult validHit = new RaycastResult();
            
            foreach (var hitResult in hitResults) if (hitResult.gameObject.tag == "OptionSelector") validHit = hitResult;
            
            if (!validHit.isValid) 
            {
                transform.SetParent(originalParent);
                transform.position = originalPosition;
                return;
            }
            
            transform.SetParent(validHit.gameObject.transform);
            (transform as RectTransform).anchoredPosition = new Vector2(0f, 0f);
            
            var optionSelector = validHit.gameObject.GetComponent<OptionSelector>();
            optionSelector.SetSelectedOption(optionSO);
        }
    }
}
