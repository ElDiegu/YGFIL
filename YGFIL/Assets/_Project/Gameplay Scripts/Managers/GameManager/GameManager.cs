using UnityEngine;
using YGFIL.ScriptableObjects;

namespace YGFIL.Managers
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        [field: SerializeField]
        public MonsterSO Monster { get; private set; }
    }
}
