using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace YGFIL
{
    public class AffinityTestManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text questionText;
        [SerializeField] private TMP_Text[] optionButtonTexts;

        int questionCounter = 0;
        int[] options = {0, 1, -1, 0};

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
            questionText.text = "Preguntoto " + questionCounter;
            for (int i = 0; i < options.Length; i++)
            {
                optionButtonTexts[i].text = "opsion " + i;
            }
            questionCounter++;
        }

        public void OnOptionButtonPressed(int optionClicked) 
        {
            switch (options[optionClicked]) 
            {
                case 1:

                    break;

                case -1:

                    break;

                case 0:

                    break;
            }

            if (questionCounter < 3) 
            {
                SetQuestion();
            }
            else
            {
                this.gameObject.SetActive(false);
                //end Test Afinidad
            }
        }
    }
}
