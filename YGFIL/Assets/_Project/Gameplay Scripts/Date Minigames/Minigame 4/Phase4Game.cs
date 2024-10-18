using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace YGFIL
{
    public class Phase4Game : MonoBehaviour
    {
        [SerializeField] private TMP_Text textGame4;
        [SerializeField] private TMP_Text fillText;
        [SerializeField] private TMP_Text[] optionButtonTexts;
        [SerializeField] private Button submitButton;

        int[] options = {0, 1, -1};
        int currentOption = -1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnEnable()
        {
            SetQuestion();
        }


        private void SetQuestion()
        {
            textGame4.text = "Coso que contar: _______";
            for (int i = 0; i < options.Length; i++)
            {
                optionButtonTexts[i].text = "opsion " + i;
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
            switch (options[currentOption]) 
            {
                case 1:

                    break;

                case -1:

                    break;

                case 0:

                    break;
            }

            this.gameObject.SetActive(false);
            //end Fase 4
        }
    }
}
