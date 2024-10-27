using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YGFIL.Databases;
using YGFIL.Managers;
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
        
        private void Update()
        {
            if (pressCount >= 3 && IceBreakingManager.Instance.selectedOption != iceOptionSO) button.image.sprite = ImagesDatabase.IceSprites[iceOptionSO.NoteType][pressCount];
        }

        public void PressOption() 
        {
            if (pressCount >= 3) 
            {
                SelectOption();
                button.image.sprite = ImagesDatabase.IceSprites[iceOptionSO.NoteType][4];
                return;
            }
            
            if (Random.Range(0, 100) < chance) 
            {
                AudioManager.Instance.Play("iceHit");
                pressCount++;
                chance = 20;  
            } 
            else chance += 10f;
            
            button.image.sprite = ImagesDatabase.IceSprites[iceOptionSO.NoteType][pressCount];
            
            if (pressCount >= 3) 
            {
                AudioManager.Instance.Play("iceBreak");
                var textObject = transform.GetChild(iceOptionSO.NoteType).gameObject; 
                textObject.SetActive(true);
                textObject.GetComponent<TextMeshProUGUI>().text = iceOptionSO.Text;
            }
        }
        
        public void LastImage() 
        {
            
        }
        
        private void SelectOption() => IceBreakingManager.Instance.ChangeSelectedOption(iceOptionSO);
    }
}
