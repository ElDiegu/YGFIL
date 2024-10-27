using System.Collections.Generic;
using UnityEngine;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(menuName = "You're Gonna Fall: In Love/Phase4Game/Option")]
    public class Phase4OptionSO : ScriptableObject 
    {
        [field: SerializeField]
        public Sprite Image { get; private set; }
        
        [field: SerializeField]
        public string Text { get; private set; }

        [field: SerializeField]
        public Color TextColor { get; private set; }

        [field: SerializeField]
        public int LoveValue { get; private set; }
    }
}
