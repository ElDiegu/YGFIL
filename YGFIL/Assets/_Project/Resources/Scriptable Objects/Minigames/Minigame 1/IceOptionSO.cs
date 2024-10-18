using System.Collections.Generic;
using UnityEngine;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(menuName = "You're Gonna Fall: In Love/Minigames/Minigame 1 Option")]
    public class IceOptionSO : ScriptableObject 
    {
        [field: SerializeField]
        public List<Sprite> Images { get; private set; }

        [field: SerializeField]
        public int LoveValue { get; private set; }
    }
}
