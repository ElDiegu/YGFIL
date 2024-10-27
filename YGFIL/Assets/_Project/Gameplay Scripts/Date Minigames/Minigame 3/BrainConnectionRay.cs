using System.Collections.Generic;
using Systems.EventSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YGFIL.Events;
using YGFIL.Managers;
using YGFIL.Managers.Minigames;

namespace YGFIL.Minigames
{
    public class BrainConnectionRay : MonoBehaviour, IDraggable
    {
        [SerializeField] private UILineRenderer line;
        [SerializeField] private GraphicRaycaster raycaster;
        [SerializeField] string FailTag, SuccessTag;
        [SerializeField] private MazeManager mazeManager;
        [SerializeField] private GameObject previousCell;
        
        EventBinding<GeneratedMazeEvent> generatedMazeEventBinding;
        
        private void OnEnable()
        {
            generatedMazeEventBinding = new EventBinding<GeneratedMazeEvent>(GenerateStartLine);
            EventBus<GeneratedMazeEvent>.Register(generatedMazeEventBinding);
        }
        
        private void OnDisable() 
        {
            EventBus<GeneratedMazeEvent>.Deregister(generatedMazeEventBinding);
        }
        
        private void GenerateStartLine(GeneratedMazeEvent generatedMazeEvent)
        {
            var startPosition = generatedMazeEvent.startCell;
            
            var point2 = startPosition.anchoredPosition;
            var point1 = point2 + new Vector2(0f, 70f);
            
            line.AddPointToLine(point1);
            line.AddPointToLine(point2);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }

        public void OnDrag(PointerEventData eventData)
        {
            List<RaycastResult> hitResults = new List<RaycastResult>();
            
            raycaster.Raycast(eventData, hitResults);
            
            Debug.Log(hitResults.Count);
            
            foreach (var hit in hitResults) 
                if (hit.gameObject.tag == SuccessTag)
                {
                    if (previousCell == null) previousCell = hit.gameObject;
                    
                    var currentNode = mazeManager.GetNodeFromChildIndex(hit.gameObject.transform.GetSiblingIndex());
                    var previousNode = mazeManager.GetNodeFromChildIndex(previousCell.transform.GetSiblingIndex());
                    
                    if (!mazeManager.IsAdjacent(currentNode, previousNode)) continue;
                    
                    var hitPosition = (hit.gameObject.transform as RectTransform).anchoredPosition;
                    
                    line.AddPointToLine(hitPosition);
                    previousCell = hit.gameObject;
                    
                    if (mazeManager.GetNodeFromChildIndex(hit.gameObject.transform.GetSiblingIndex()) != mazeManager.end) continue;
                    
                    RevertConnections();
                    AudioManager.Instance.Play("brainMazeCompleted");
                    eventData.pointerDrag = null;
                    mazeManager.GenerateMaze();
                    EventBus<SolvedMazeEvent>.Raise(new SolvedMazeEvent());
                }
                else if(hit.gameObject.tag == FailTag) 
                {
                    RevertConnections();
                    eventData.pointerDrag = null;
                    //ExecuteEvents.endDragHandler.Invoke(this, new PointerEventData(EventSystem.current));
                }
                    
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }
        
        private void RevertConnections() 
        {
            line.RemovePointsFromLine(2, line.points.Count - 2);
            previousCell = null;
        }
    }
}
