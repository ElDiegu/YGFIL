using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Systems.EventSystem;
using TMPro;
using UnityEngine;
using YGFIL.Events;
using YGFIL.ScriptableObjects;

namespace YGFIL
{
    public class IceBreakingManager : StaticInstance<IceBreakingManager>, ISOContainer
    {
        [SerializeField] private IceOptionsSetSO setOptions;
        public ScriptableObject ScriptableObject { get => setOptions; set {} }

        [SerializeField] private IceOptionSO selectedOption;
        
        public void ChangeSelectedOption(IceOptionSO option) => selectedOption = option;

        public void SubmitOption() 
        {
            EventBus<UpdateLoveValueEvent>.Raise(new UpdateLoveValueEvent() 
            {
                loveValue = selectedOption.LoveValue
            });
        }
    }
}
