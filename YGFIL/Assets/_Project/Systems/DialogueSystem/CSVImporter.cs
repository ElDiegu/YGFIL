using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVImporter
{
    
    public static List<DialogueData> GetDialogueData(string filePath)
    {
        List<string[]> csvData = CSVReader.ReadCSV(filePath);
        List<DialogueData> data = new List<DialogueData>();
        for(int i = 1; i < csvData.Count; i++)
        {
            string[] row = csvData[i];

            string id = row[0];
            string speaker = row[1];
            string text = row[2].Trim('"');
            string expression = row[3];
            string type = row[4];
            string nextIndex = row[5];

            DialogueData dialogue = new DialogueData(id, speaker, text, expression, type, nextIndex);

            data.Add(dialogue);
        }

        return data;
    }
}
