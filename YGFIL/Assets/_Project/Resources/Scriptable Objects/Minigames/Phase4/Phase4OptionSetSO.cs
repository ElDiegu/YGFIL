using System.Collections.Generic;
using UnityEngine;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(menuName = "You're Gonna Fall: In Love/Phase4Game/Option Set")]
    public class Phase4OptionSetSO : ScriptableObject
    {
        [field: SerializeField]
        public List<Phase4OptionSO> Options { get; private set; } = new List<Phase4OptionSO>(3);

        [field: SerializeField]
        public string CardText { get; private set; }

        [field: SerializeField]
        public Sprite CardImage { get; private set; }
    }
}
