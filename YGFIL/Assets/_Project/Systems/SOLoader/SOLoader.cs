using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YGFIL.ScriptableObjects;
using YGFIL.Systems;

namespace YGFIL.Systems
{
    public class SOLoader
    {
        public static string loadingPath = "Assets/_Project/Resources/ScriptableObjectsTSV/";
        public static string savingPath = "Assets/_Project/Resources/Scriptable Objects/";
        public static List<string> SOPaths = new List<string>() 
        {
            "IceOption"
        };
        
        [InitializeOnLoadMethod]
        public static void LoadAllSO() 
        {
            LoadIceOption(loadingPath, savingPath);
        }
        
        public static void LoadIceOption(string loadingPath, string savingPath) 
        {
            var parsedData = FileParser.ParseFile(loadingPath + "IceOptionSO.tsv", "\t");
            
            for (int i = 1; i < parsedData.Count; i++) AssetDatabase.CreateAsset(new IceOptionSO(parsedData[i]), savingPath + "IceBreaking/" + parsedData[i][0] + ".asset");
        }
    }
}
