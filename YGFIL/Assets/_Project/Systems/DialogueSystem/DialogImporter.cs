using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using YGFIL.Databases;
using YGFIL.Monsters;

namespace YGFIL.Systems
{
    public class DialogImporter
    {
#if UNITY_EDITOR
        private static string path = "Assets/_Project/Resources/DialogCSV/";
#else
        private static string path = "Dialogs/";
#endif
        public static List<DialogData> ImportDialog(string fileName) 
        {
            var parsedData = FileParser.ParseFile(path + fileName + ".tsv", "\t");
            List<DialogData> data = new List<DialogData>();
            
            for (int i = 1; i < parsedData.Count; i++)
            {
                string[] row = parsedData[i];
                
                int id = int.Parse(row[0]);
                MonsterType monster = (MonsterType)Enum.Parse(typeof(MonsterType), row[1]);
                MonsterExpressionType expression = (MonsterExpressionType)Enum.Parse(typeof(MonsterExpressionType), row[2]);
                string text = row[3];
                
                int nextID = -1;
                if(int.TryParse(row[4], out int number)) nextID = number;
                
                
                List<string> tags = new List<string>();
                for (int j = 5; j < row.Length; j++) if(row[j] != "") tags.Add(row[j]);

                DialogData dialogue = new DialogData(id, monster, expression, text, nextID, tags);

                data.Add(dialogue);
            }

            return data;
        }
    }
}
