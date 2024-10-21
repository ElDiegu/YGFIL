using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using System.Text.RegularExpressions;
using System.Text;

public class CSVReader
{
    public static List<string[]> ReadCSV(string filePath)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(filePath);
        string initContent = csvFile.text;
        string csvContent = csvFile.text;
        csvContent = Regex.Replace(csvContent, ";(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)", "|||");

        File.WriteAllText(filePath, csvContent, Encoding.UTF8);

        List<string[]> data = new List<string[]>();
        
        using (StreamReader reader = new StreamReader(filePath))
        {
            while(!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split("|||");

                data.Add(values);
            }
        }
        File.WriteAllText(filePath, initContent, Encoding.UTF8);
        return data;
    }
}
