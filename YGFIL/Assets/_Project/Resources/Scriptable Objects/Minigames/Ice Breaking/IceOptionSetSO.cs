using System.Collections.Generic;
using UnityEngine;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(menuName = "You're Gonna Fall: In Love/Ice Breaking/Option Set")]
    public class IceOptionSetSO : ScriptableObject
    {
        [field: SerializeField]
        public List<IceOptionSO> Options { get; private set; } = new List<IceOptionSO>(3);
    }
}
