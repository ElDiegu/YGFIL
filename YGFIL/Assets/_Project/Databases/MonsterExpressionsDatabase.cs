using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;
using YGFIL.Monsters;

namespace YGFIL.Databases
{
    public class MonsterExpressionDatabase
    {
        public static Dictionary<MonsterType, List<Sprite>> MonsterExpressions = new Dictionary<MonsterType, List<Sprite>>();
        
        [InitializeOnLoadMethod]
        private static void PopulateExpressionsDictionary() 
        {
            Debug.Log("Populating Expressions Dictionary");
            
            var spriteSheets = Resources.LoadAll<Sprite>("Art/Monsters").ToList();
            
            foreach (Sprite sprite in spriteSheets) 
            {
                var data = sprite.name.Split("_");
                
                var monster = (MonsterType)Enum.Parse(typeof(MonsterType), data[0]);
                
                if (!MonsterExpressions.ContainsKey(monster)) MonsterExpressions.Add(monster, new List<Sprite>());
                
                MonsterExpressions[monster].Add(sprite);
            }
            //MonsterExpressions.Add(monsterType, Resources.LoadAll<Sprite>("Art/Monsters/" + monsterType.ToString()).ToList());
            
        }
        
        public static Sprite GetSprite(int monsterType, int expressionType) => MonsterExpressions[(MonsterType)monsterType][expressionType];
        public static Sprite GetSprite(MonsterType monsterType, MonsterExpressionType expressionType) => MonsterExpressions[monsterType][(int)expressionType];
    }

    public enum MonsterExpressionType
    {
        Neutral,
        Happy,
        Dislike
    }
}
