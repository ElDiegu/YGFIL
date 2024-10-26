using System.Collections.Generic;
using UnityEngine;
using YGFIL.Databases;
using YGFIL.Monsters;

namespace YGFIL.Systems
{
    public class DialogData
    {
        public int ID { get; private set; }
        public MonsterType Monster { get; private set; }   
        public MonsterExpressionType Expression { get; private set; }
        public string Text { get; private set; }
        public int NextID { get; private set; }
        public List<string> Tag { get; private set; }

        public DialogData(int id, MonsterType monster, MonsterExpressionType expression, string text, int nextID, List<string> tag)
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

namespace YGFIL.Enums 
{
    public enum DialogTag 
    {
        End,
        IceBreaking_Start,
        IceBreaking_End,
        IceBreaking_Good,
        IceBreaking_Neutral,
        IceBreaking_Bad,
        Introductions_Start,
        Introductions_Good,
        Introductions_Bad,
        Brain_Start,
        Brain_End,
        Brain_One,
        Brain_Two,
        Brain_Three,
        Brain_Four
    }
}
