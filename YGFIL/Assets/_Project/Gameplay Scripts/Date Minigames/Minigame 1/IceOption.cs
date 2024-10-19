using UnityEngine;
using UnityEngine.UI;
using YGFIL.ScriptableObjects;

namespace YGFIL
{
    public class IceOption : MonoBehaviour, ISOContainer
    {
        [SerializeField] private IceOptionSO iceOptionSO;
        public ScriptableObject ScriptableObject { get => iceOptionSO; set {} }
        
        [SerializeField] private Button button;
        [SerializeField] private int pressCount;

        [SerializeField] private int buttonIndex;

        private void Start()
        {
            buttonIndex = transform.GetSiblingIndex();
            
            iceOptionSO = (IceBreakingManager.Instance.ScriptableObject as IceOptionsSetSO).Options[buttonIndex];
            
            button.image.sprite = iceOptionSO.Images[0];
        }

        public void PressOption() 
        {
            if (pressCount >= iceOptionSO.Images.Count - 1) SelectOption();
            else 
            {
                pressCount++;
                button.image.sprite = iceOptionSO.Images[pressCount];
            }
        }
        
        private void SelectOption() => IceBreakingManager.Instance.ChangeSelectedOption(iceOptionSO);
    }
}
