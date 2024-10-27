using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace YGFIL.Databases
{
    public class ImagesDatabase
    {
        public static List<Sprite> IntroductionsSprites { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void PopulateDatabase() 
        {
            IntroductionsSprites = Resources.LoadAll<Sprite>("Art/Minigames").Where(sprite => sprite.name.Contains("Note")).ToList();
        }
    }
}
