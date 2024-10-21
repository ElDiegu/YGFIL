using UnityEngine;

namespace YGFIL
{
    [CreateAssetMenu(fileName = "BrainMazeOption", menuName = "You're Gonna Fall: In Love/Brain Connections/Brain Connections Option")]
    public class BrainConnectionsOptionSO : ScriptableObject
    {
        [field: SerializeField]
        public string Text { get; private set; }

        [field: SerializeField]
        public int LoveValue { get; private set; }
    }
}