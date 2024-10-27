using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YGFIL.Minigames.Managers;
using YGFIL.ScriptableObjects;
using YGFIL.Enums;

namespace YGFIL
{
    public class Phase4Game : MonoBehaviour
    {
        [SerializeField] private TMP_Text textGame4;
        [SerializeField] private TMP_Text fillText;
        [SerializeField] private TMP_Text[] optionButtonTexts;
        [SerializeField] private Button submitButton;

        [SerializeField] private Phase4OptionSetSO optionsSet;
        int currentOption = -1;

        private void Start()
        {
            textGame4.text = optionsSet.CardText;
            for (int i = 0; i < 3; i++)
            {
                optionButtonTexts[i].text = optionsSet.Options[i].Text;
            }
        }

        public void OnOptionButtonPressed(int optionClicked)
        {
            currentOption = optionClicked;
            fillText.text = optionButtonTexts[optionClicked].text;
            submitButton.enabled = true;
        }
        public void OnSubmitButtonPressed() 
        {
            submitButton.enabled = false;
            Phase4GameManager.Instance.SubmitOption(currentOption);
        }
    }
}
