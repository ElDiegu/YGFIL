using UnityEngine;
using YGFIL.ScriptableObjects;

namespace YGFIL.Minigames.PhaseTwo
{
    public class OptionSelector : MonoBehaviour
    {
        [SerializeField] private IntroductionOptionSO optionSO;
        
        public void SetSelectedOption(IntroductionOptionSO optionSO) => this.optionSO = optionSO;
        
        public IntroductionOptionSO GetSelectedOption() => optionSO;
    }
}
