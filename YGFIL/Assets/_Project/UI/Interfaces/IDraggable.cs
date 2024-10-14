using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YGFIL
{
    public interface IDraggable : IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        
    }
}
