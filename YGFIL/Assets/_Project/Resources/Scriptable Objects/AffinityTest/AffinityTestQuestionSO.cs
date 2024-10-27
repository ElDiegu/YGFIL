using System.Collections.Generic;
using UnityEngine;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(menuName = "You're Gonna Fall: In Love/AffinityTest/Question")]
    public class AffinityTestQuestionSO : ScriptableObject 
    {       
        [field: SerializeField]
        public string QuestionText { get; private set; }

        [field: SerializeField]
        public List<string> OptionsTexts { get; private set; }

        [field: SerializeField]
        public List<int> LoveValues { get; private set; }
        
        public AffinityTestQuestionSO(string[] parameters) 
        {
            QuestionText = parameters[1];
            
            for (int i = 0; i < 4; i++) 
            {
                OptionsTexts.Add(parameters[2 + i]);
                LoveValues.Add(int.Parse(parameters[3 + i]));
            }
        }
    }
}