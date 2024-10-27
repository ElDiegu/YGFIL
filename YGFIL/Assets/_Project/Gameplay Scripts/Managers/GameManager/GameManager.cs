using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using YGFIL.Monsters;
using YGFIL.ScriptableObjects;

namespace YGFIL.Managers
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        [field: SerializeField]
        public MonsterSO Monster { get; private set; }
        private List<MonsterSO> monsterList;

        public void setMonster(int monsterId)
        {
            Monster = monsterList[monsterId];
        }
    }
}
