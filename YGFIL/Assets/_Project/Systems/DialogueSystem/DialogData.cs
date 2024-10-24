using UnityEngine;
using YGFIL.Databases;
using YGFIL.Monsters;

namespace YGFIL
{
    public class DialogData
    {
        public int ID { get; private set; }
        public MonsterType Monster { get; private set; }   
        public MonsterExpressionType Expression { get; private set; }
        public string Text { get; private set; }
        public int NextID { get; private set; }
        public string Tag { get; private set; }

        public DialogData(int id, MonsterType monster, MonsterExpressionType expression, string text, int nextID, string tag)
        {
            ID = id;
            Monster = monster;
            Expression = expression;
            Text = text;
            NextID = nextID;
            Tag = tag;
        }

        public override string ToString()
        {
            return $"{ID} | {Monster} | {Expression} | {Text} | {NextID} | {Tag}";
        }
    }
}
