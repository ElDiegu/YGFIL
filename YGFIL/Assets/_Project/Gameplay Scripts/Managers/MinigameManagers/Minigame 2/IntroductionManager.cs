using System.Collections.Generic;
using Systems.EventSystem;
using UnityEngine;
using YGFIL.Events;
using YGFIL.ScriptableObjects;

namespace YGFIL.Minigames.Managers
{
    public class IntroductionManager : StaticInstance<IntroductionManager>
    {
        [SerializeField] private List<IntroductionOptionSO> introductionOptions;
        
        public void ChangeOptionStatus(IntroductionOptionSO introductionOption, bool selected) 
        {
            if (selected && !introductionOptions.Contains(introductionOption)) introductionOptions.Add(introductionOption);
            else if (!selected && introductionOptions.Contains(introductionOption)) introductionOptions.Remove(introductionOption);
        }
        
        public void SubmitIntroduction() 
        {
            var totalLoveValue = 0;
            foreach (IntroductionOptionSO introduction in introductionOptions) totalLoveValue += introduction.LoveValue;
            
            EventBus<UpdateLoveValueEvent>.Raise(new UpdateLoveValueEvent() 
            {
                loveValue = totalLoveValue
            });
        }
    }
}
