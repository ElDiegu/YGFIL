using System.Collections;
using UnityEngine;

namespace YGFIL.Utils
{
    public class SlideUtils
    {
        public static IEnumerator SlideCoroutineWithAnchoredPosition(GameObject callingObject, Vector2 targetPosition, float speed, float speedMultiplier) 
        {
            while ((callingObject.transform as RectTransform).anchoredPosition != targetPosition) 
            {
                var currentPosition = (callingObject.transform as RectTransform).anchoredPosition;
                
                var multiplier = Mathf.Max(0.1f, Mathf.Abs(currentPosition.y - targetPosition.y) * speedMultiplier);
                
                var newPosition = Mathf.MoveTowards((callingObject.transform as RectTransform).anchoredPosition.y, targetPosition.y, speed * multiplier * Time.deltaTime);
                
                (callingObject.transform as RectTransform).anchoredPosition = new Vector2(currentPosition.x, newPosition);
                
                yield return null;
            }
            
            yield return null;
        }
    }
}
