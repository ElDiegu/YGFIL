using System.Collections.Generic;
using UnityEngine;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(menuName = "You're Gonna Fall: In Love/Minigames/Minigame 1 Option Set")]
    public class IceOptionsSetSO : ScriptableObject
    {
        [field: SerializeField]
        public List<IceOptionSO> Options { get; private set; } = new List<IceOptionSO>(3);
    }
}
