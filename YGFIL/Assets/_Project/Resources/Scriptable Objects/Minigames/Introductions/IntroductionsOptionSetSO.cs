using System.Collections.Generic;
using UnityEngine;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(fileName = "IntroductionsOptionSetSO", menuName = "You're Gonna Fall: In Love/Introductions/Option Set")]
    public class IntroductionsOptionSetSO : ScriptableObject
    {
        [field: SerializeField]
        public List<IntroductionsOptionSO> Options { get; private set; }
    }
}
