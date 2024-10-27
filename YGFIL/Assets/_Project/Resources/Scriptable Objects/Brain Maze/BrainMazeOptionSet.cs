using System.Collections.Generic;
using UnityEngine;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BrainMazeOptionSet", menuName = "You're Gonna Fall: In Love/Brain Connections/Brain Connections Option Set")]
    public class BrainConnectionsOptionSetSO : ScriptableObject
    {
        [field: SerializeField]
        public List<BrainConnectionsOptionSO> Options { get; private set; } = new List<BrainConnectionsOptionSO>(4);
    }
}
