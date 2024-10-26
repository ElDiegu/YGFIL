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
    }
}
