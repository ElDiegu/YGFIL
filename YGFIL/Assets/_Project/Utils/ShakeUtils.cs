using System.Collections;
using UnityEngine;

namespace YGFIL.Utils
{
    public class ShakeUtils
    {
        public static IEnumerator BeatingImageCoroutine(Transform transform, float maxScale, float minScale, float speed)
        {
            Vector3 targetScale = new Vector2(minScale, minScale);

            while (true)
            {
                while (transform.localScale != targetScale)
                {
                    var scale = Mathf.MoveTowards(transform.localScale.x, targetScale.x, speed * Time.deltaTime);
                    
                    transform.localScale = new Vector2(scale, scale);
                    
                    yield return null;
                }
                
                targetScale = transform.localScale.x == minScale ? new Vector2(maxScale, maxScale) : new Vector2(minScale, minScale);
                
                yield return null;
            }
        }
    }
}