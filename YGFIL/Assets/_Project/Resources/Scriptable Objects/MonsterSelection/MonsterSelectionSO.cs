using UnityEngine;

namespace YGFIL
{
    [CreateAssetMenu(fileName = "MonsterSelectionSO", menuName = "You're Gonna Fall: In Love/MonsterSelectionSO")]
    public class MonsterSelectionSO : ScriptableObject
    {
        [field: SerializeField]
        public Sprite FaceImage { get; private set; }

        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public string MonsterType { get; private set; }

        [field: SerializeField]
        public Sprite SelectorImage { get; private set; }

        [field: SerializeField]
        public int datesCompletedNeeded { get; private set; }
    }
}
