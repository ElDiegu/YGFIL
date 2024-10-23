using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor.UI;
using System.Collections.Generic;
using UnityEngine.UI;
using YGFIL.ScriptableObjects;
using YGFIL.Minigames.Managers;
using UnityEditor;

namespace YGFIL.Minigames.PhaseTwo
{
    public class IntroductionOptionCard : MonoBehaviour, IDraggable, ISOContainer
    {
        [SerializeField] private IntroductionOptionSO optionSO;
        public ScriptableObject ScriptableObject { get => optionSO; set {} }
        
        [SerializeField] private Vector3 originalPosition;
        [SerializeField] private Transform originalParent;
        
        Physics physics;
        
        private void Awake()
        {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.00000000001f;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            originalPosition = transform.position;
            originalParent = transform.parent;
            
            transform.SetParent(transform.parent.transform.parent);
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
            
            if (hitResults.Count <= 0) 
            {
                transform.SetParent(originalParent);
                transform.position = originalPosition;
                return;
            }
            
            foreach (var hitResult in hitResults) 
                if (hitResult.gameObject.tag == "IntroductionContainer") DroppedOnSelector(hitResult);
                else if (hitResult.gameObject.tag == "IntroductionMenu") DroppedOnMenu(hitResult);
                else Debug.Log(hitResult.gameObject.name);
        }
        
        private void DroppedOnSelector(RaycastResult hit) 
        {
            transform.SetParent(hit.gameObject.transform);
            (transform as RectTransform).anchoredPosition = new Vector2(0f, 0f);
            
            IntroductionManager.Instance.ChangeOptionStatus(optionSO, true);
        }
        
        private void DroppedOnMenu(RaycastResult hit) 
        {
            transform.SetParent(hit.gameObject.transform);
            
            IntroductionManager.Instance.ChangeOptionStatus(optionSO, false);
        }
    }
}
