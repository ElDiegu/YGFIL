using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using YGFIL.ScriptableObjects;
using YGFIL.Minigames.Managers;
using UnityEditor;
using TMPro;
using YGFIL.Databases;
using YGFIL.Utils;
using System.Collections;
using YGFIL.Managers;

namespace YGFIL.Minigames
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
        
        private IEnumerator Start() 
        {
            var index = transform.GetSiblingIndex() - 1;
            
            while (IntroductionsManager.Instance.ScriptableObject == null) yield return null;
            
            optionSO = ((IntroductionsOptionSetSO)IntroductionsManager.Instance.ScriptableObject).Options[index];
            
            image.sprite = ImagesDatabase.IntroductionsSprites[optionSO.ImageIndex];
            
            image.SetNativeSize();
            
            var textIndex = Mathf.FloorToInt(optionSO.ImageIndex / 3);
            
            transform.GetChild(textIndex).gameObject.SetActive(true);
            transform.GetChild(textIndex).GetComponent<TextMeshProUGUI>().color = optionSO.TextColor;
            transform.GetChild(textIndex).GetComponent<TextMeshProUGUI>().text = optionSO.Text;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            originalPosition = (transform as RectTransform).anchoredPosition;
            originalParent = transform.parent;
            AudioManager.Instance.Play("papper");

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

            AudioManager.Instance.Play("napkin");
            var graphicRaycaster = transform.root.GetComponent<GraphicRaycaster>();
            graphicRaycaster.Raycast(eventData, hitResults);
            
            if (hitResults.Count <= 0) 
            {
                transform.SetParent(originalParent);
                (transform as RectTransform).anchoredPosition = originalPosition;
                return;
            }
            
            bool validHit = false;
            
            foreach (var hitResult in hitResults) 
                if (hitResult.gameObject.tag == "IntroductionContainer") 
                {
                    DroppedOnSelector(hitResult);
                    validHit = true;
                    break;    
                } 
                else if (hitResult.gameObject.tag == "IntroductionMenu") 
                {
                    DroppedOnMenu(hitResult);
                    validHit = true;
                    break;   
                }
            
            if (!validHit) ResetCard();
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
        private void ResetCard() 
        {
            transform.SetParent(originalParent);
            (transform as RectTransform).anchoredPosition = originalPosition;
        }
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
