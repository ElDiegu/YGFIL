using System.Collections.Generic;
using UnityEngine;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(menuName = "You're Gonna Fall: In Love/AffinityTest/Question Set")]
    public class AffinityTestQuestionSetSO : ScriptableObject
    {
        [field: SerializeField]
        public List<AffinityTestQuestionSO> Questions { get; private set; } = new List<AffinityTestQuestionSO>(4);
    }
}
