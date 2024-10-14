using UnityEngine;
using YGFIL.ScriptableObjects;

namespace YGFIL.Minigames.PhaseTwo
{
    public class OptionSelector : MonoBehaviour
    {
        [SerializeField] private OptionSO optionSO;
        
        public void SetSelectedOption(OptionSO optionSO) => this.optionSO = optionSO;
        
        public OptionSO GetSelectedOption() => optionSO;
    }
}
