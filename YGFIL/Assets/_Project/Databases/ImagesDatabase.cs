using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace YGFIL.Databases
{
    public class ImagesDatabase
    {
        public static List<Sprite> IntroductionsSprites { get; private set; } = new List<Sprite>();
        public static Dictionary<int, Sprite[]> IceSprites { get; private set; } = new Dictionary<int, Sprite[]>()
        {
            {0, new Sprite[4]}, {1, new Sprite[4]}, {2, new Sprite[4]}, {3, new Sprite[4]},
        };
        public static List<Sprite> DialogSprites { get; private set; } = new List<Sprite>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void PopulateDatabase() 
        {
            IntroductionsSprites = Resources.LoadAll<Sprite>("Art/Minigames").Where(sprite => sprite.name.Contains("Note")).ToList();
            
            var iceImages = Resources.LoadAll<Sprite>("Art/Minigames").Where( sprite => sprite.name.Contains("Ice")).ToList();
            
            foreach (var iceImage in iceImages) 
            {
                var info = iceImage.name.Split("_");
                
                IceSprites[int.Parse(info[1])][int.Parse(info[2])] = iceImage;
            }
            
            DialogSprites = Resources.LoadAll<Sprite>("Art/UI/").Where(sprite => sprite.name.Contains("DialogUI")).ToList();
        }
    }
}
