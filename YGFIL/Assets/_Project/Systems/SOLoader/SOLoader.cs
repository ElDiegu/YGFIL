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
        
        public static Dictionary<string, bool> DirtyDictionary = new Dictionary<string, bool>() 
        {
            {"IceBreakingSO", false},
            {"IntroductionsOptionSO", false},
            {"BrainMazeOptionSO", false},
            {"Phase4Option", false},
            {"AffinityTestOption", false}
        };

#if UNITY_EDITOR        
        [InitializeOnLoadMethod]
        public static void LoadAllSO() 
        {
            if (DirtyDictionary["IceBreakingSO"]) LoadIceOption(loadingPath, savingPath);
            if (DirtyDictionary["IntroductionsOptionSO"]) LoadIntroductionsSO(loadingPath, savingPath);
            if (DirtyDictionary["BrainMazeOptionSO"]) LoadBrainMazeSO(loadingPath, savingPath);
            if (DirtyDictionary["Phase4Option"]) LoadPhase4SO(loadingPath, savingPath);
            if (DirtyDictionary["AffinityTestOption"]) LoadAffinityTestSO(loadingPath, savingPath);
        }

        public static void LoadIceOption(string loadingPath, string savingPath) 
        {
            var parsedData = FileParser.ParseFile(loadingPath + "IceOptionSO.tsv", "\t");
            
            for (int i = 1; i < parsedData.Count; i++) AssetDatabase.CreateAsset(new IceOptionSO(parsedData[i]), savingPath + "IceBreaking/" + parsedData[i][0] + ".asset");
        }
        
        public static void LoadIntroductionsSO(string loadingPath, string savingPath) 
        {
            var parsedData = FileParser.ParseFile(loadingPath + "IntroductionsOptionSO.tsv", "\t");
            
            for (int i = 1; i < parsedData.Count; i++) AssetDatabase.CreateAsset(new IntroductionsOptionSO(parsedData[i]), savingPath + "Introductions/" + parsedData[i][0] + ".asset");
        }
        
        public static void LoadBrainMazeSO(string loadingPath, string savingPath) 
        {
            var parsedData = FileParser.ParseFile(loadingPath + "BrainMazeOptionsSO.tsv", "\t");
            
            for (int i = 1; i < parsedData.Count; i++) AssetDatabase.CreateAsset(new BrainConnectionsOptionSO(parsedData[i]), savingPath + "Brain Maze/" + parsedData[i][0] + ".asset");
        }
        
        public static void LoadPhase4SO(string loadingPath, string savingPath) 
        {
            var parsedData = FileParser.ParseFile(loadingPath + "Phase4OptionSO.tsv", "\t");
            
            for (int i = 1; i < parsedData.Count; i++) AssetDatabase.CreateAsset(new Phase4OptionSO(parsedData[i]), savingPath + "Phase4/" + parsedData[i][0] + ".asset");
        }
        
        public static void LoadAffinityTestSO(string loadingPath, string savingPath) 
        {
            var parsedData = FileParser.ParseFile(loadingPath + "AffinityTestQuestionSO.tsv", "\t");
            
            for (int i = 1; i < parsedData.Count; i++) AssetDatabase.CreateAsset(new AffinityTestQuestionSO(parsedData[i]), savingPath + "AffinityTest/" + parsedData[i][0] + ".asset");
        }
#endif
    }
}
