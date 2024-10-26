using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor.UI;
using System.Collections.Generic;
using UnityEngine.UI;
using YGFIL.ScriptableObjects;
using YGFIL.Minigames.Managers;
using UnityEditor;
using TMPro;
using YGFIL.Databases;
using YGFIL.Utils;

namespace YGFIL.Minigames.PhaseTwo
{
    public class IntroductionsOptionCard : MonoBehaviour, IDraggable, ISOContainer
    {
        [SerializeField] private IntroductionsOptionSO optionSO;
        public ScriptableObject ScriptableObject { get => optionSO; set {} }
        
        [SerializeField] private Image image;
        [SerializeField] private Vector2 slidePosition;
        [SerializeField] private float slideSpeed, slideMultiplier;
        private Coroutine slideCoroutine;
        
        private Vector3 originalPosition;
        private Transform originalParent;
        
        private void Awake()
        {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.00000000001f;
        }
        
        private void Start() 
        {
            var index = transform.GetSiblingIndex() - 1;
            optionSO = ((IntroductionsOptionSetSO)IntroductionsManager.Instance.ScriptableObject).Options[index];
            
            image.sprite = ImagesDatabase.IntroductionsSprites[optionSO.ImageIndex];
            
            var textIndex = Mathf.FloorToInt(optionSO.ImageIndex / 3);
            
            transform.GetChild(textIndex).gameObject.SetActive(true);
            transform.GetChild(textIndex).GetComponent<TextMeshProUGUI>().color = optionSO.TextColor;
            transform.GetChild(textIndex).GetComponent<TextMeshProUGUI>().text = optionSO.Text;
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
            
            IntroductionsManager.Instance.ChangeOptionStatus(optionSO, true);
        }    
        private void DroppedOnMenu(RaycastResult hit) 
        {
            transform.SetParent(hit.gameObject.transform);
            
            IntroductionsManager.Instance.ChangeOptionStatus(optionSO, false);
        }
    
        public void Slide() 
        {
            Debug.Log($"Sliding card {name}");
            
            slideCoroutine = StartCoroutine(SlideUtils.SlideCoroutineWithAnchoredPosition(gameObject, slidePosition, slideSpeed, slideMultiplier));
        }
        public bool IsSliding() => Mathf.Abs(slidePosition.y - (transform as RectTransform).anchoredPosition.y) > 10;
    
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(IntroductionsOptionCard))]
    public class IntroductionsOptionCardEditor : Editor 
    {
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
            
            if(GUILayout.Button("Slide")) ((IntroductionsOptionCard)target).Slide();
        }
    }
#endif
}
