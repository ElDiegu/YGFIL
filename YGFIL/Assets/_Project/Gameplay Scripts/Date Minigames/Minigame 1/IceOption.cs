using UnityEngine;
using UnityEngine.UI;
using YGFIL.ScriptableObjects;

namespace YGFIL
{
    public class IceOption : MonoBehaviour, ISOContainer
    {
        [SerializeField] private IceOptionSO iceOptionSO;
        public ScriptableObject ScriptableObject { get => iceOptionSO; set {} }
        
        [SerializeField] private Image currentImage;
        private int pressCount;

        private int buttonIndex;

        private void Awake()
        {
            buttonIndex = transform.GetSiblingIndex();
            
            iceOptionSO = (IceBreakingManager.Instance.ScriptableObject as IceOptionsSetSO).Options[buttonIndex];
        }

        public void PressOption() 
        {
            if (pressCount >= iceOptionSO.Images.Count - 1) SelectOption();
            else currentImage.sprite = iceOptionSO.Images[pressCount++];
        }
        
        private void SelectOption() 
        {
            IceBreakingManager.Instance.ChangeSelectedOption(iceOptionSO);
        }
    }
}
