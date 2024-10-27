using UnityEngine;
using YGFIL.Monsters;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(menuName = "You're Gonna Fall: In Love/Monster", fileName = "NewMonster")]
    public class MonsterSO : ScriptableObject
    {
        [field: SerializeField]
        public int ID { get; private set; }
        
        [field: SerializeField]
        public string Name { get; private set; }
        
        [field: SerializeField]
        public float StartingLove { get; private set; }

        [field: SerializeField]
        public float LoveThreshold { get; private set; }
        
        [field: SerializeField]
        public Sprite Sprite { get; private set; }

        [field: SerializeField]
        public GameObject Prefab { get; set; }
        
        [field: SerializeField]
        public MonsterType MonsterType { get; private set; }
        
        [field: SerializeField]
        public IceOptionSetSO IceBreakingOptionSet { get; private set; }

        [field: SerializeField]
        public IntroductionsOptionSetSO IntroductionsOptionSet { get; set; }
        
        [field: SerializeField]
        public BrainConnectionsOptionSetSO BrainConnectionsOptionSet { get; private set;}
        
        [field: SerializeField]
        public Phase4OptionSetSO Phase4optionSet { get; set; }

        [field: SerializeField]
        public AffinityTestQuestionSetSO AffinityTestQuestionSetSO { get; set; }
    }
}
