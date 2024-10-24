using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace YGFIL.Systems
{
    public class FileParser
    {
        public static List<string[]> ParseFile(string filePath, string separatingCharacter) 
        {
            List<string[]> parsedData = new List<string[]>();
            
            using (StreamReader streamReader = new StreamReader(filePath)) 
            {
                streamReader.ReadLine(); //We need to read the first line to skip the headers
                while(!streamReader.EndOfStream) 
                {
                    string line = streamReader.ReadLine();
                    string[] parsedLine = line.Split(separatingCharacter);
                    
                    parsedData.Add(parsedLine);
                }
            }
            
            return parsedData;
        }
    }
}
