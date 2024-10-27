using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YGFIL.Databases;
using YGFIL.Minigames.Managers;
using YGFIL.ScriptableObjects;

namespace YGFIL.Minigames
{
    public class IceOption : MonoBehaviour, ISOContainer
    {
        [SerializeField] private IceOptionSO iceOptionSO;
        public ScriptableObject ScriptableObject { get => iceOptionSO; set {} }
        
        [SerializeField] private Button button;
        [SerializeField] private int pressCount;
        [SerializeField] private float chance = 20;

        [SerializeField] private int buttonIndex;

        private IEnumerator Start()
        {
            buttonIndex = transform.GetSiblingIndex();
            
            while (IceBreakingManager.Instance.ScriptableObject == null) yield return null;
            
            iceOptionSO = (IceBreakingManager.Instance.ScriptableObject as IceOptionSetSO).Options[buttonIndex];
            
            button.image.sprite = ImagesDatabase.IceSprites[iceOptionSO.NoteType][0];
        }

        public void PressOption() 
        {
            if (pressCount >= ImagesDatabase.IceSprites[iceOptionSO.NoteType].Length - 1) 
            {
                SelectOption();
                return;
            }
            
            if (Random.Range(0, 100) < chance) 
            {
                pressCount++;
                chance = 20;  
            } 
            else chance += 10f;
            
            button.image.sprite = ImagesDatabase.IceSprites[iceOptionSO.NoteType][pressCount];
            
            if (pressCount >= ImagesDatabase.IceSprites[iceOptionSO.NoteType].Length - 1) 
            {
                var textObject = transform.GetChild(iceOptionSO.NoteType).gameObject; 
                textObject.SetActive(true);
                textObject.GetComponent<TextMeshProUGUI>().text = iceOptionSO.Text;
            }
        }
        
        private void SelectOption() => IceBreakingManager.Instance.ChangeSelectedOption(iceOptionSO);
    }
}
