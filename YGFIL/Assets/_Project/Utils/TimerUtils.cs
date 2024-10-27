using System.Collections;
using Systems.EventSystem;
using TMPro;
using UnityEngine;

namespace YGFIL.Utils 
{
    public class TimerUtils 
    {
        public static IEnumerator StartTimer(TextMeshProUGUI timerText, float time) 
        {
            var timeLeft = time;
            timerText.text = timeLeft.ToString();
            
            while (timeLeft > 0) 
            {
                timeLeft -= Time.deltaTime;
                timerText.text = ((int)timeLeft).ToString();
                yield return null;
            }
            
            timerText.text = "";
            EventBus<OnTimerFinishedEvent>.Raise(new OnTimerFinishedEvent());
            
            yield return null;
        }
    }
    
    public struct OnTimerFinishedEvent : IEvent 
    {
        
    }
}