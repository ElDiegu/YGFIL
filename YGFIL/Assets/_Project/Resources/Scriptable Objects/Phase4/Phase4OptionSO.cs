using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(menuName = "You're Gonna Fall: In Love/Phase4Game/Option")]
    public class Phase4OptionSO : ScriptableObject 
    {
        [field: SerializeField]
        public Sprite Image { get; private set; }
        
        [field: SerializeField]
        public string Text { get; private set; }

        [field: SerializeField]
        public Color TextColor { get; private set; }

        [field: SerializeField]
        public int LoveValue { get; private set; }
        
        public Phase4OptionSO(string[] parameters) 
        {
            Image = Resources.LoadAll<Sprite>("Art/Minigames/").Where(sprite => sprite.name == parameters[1]).FirstOrDefault();
            Text = parameters[2];
            
            Color textColor = new Color();
            bool valid = ColorUtility.TryParseHtmlString(parameters[3], out textColor);
            if (valid) TextColor = textColor;
            else TextColor = Color.black;
            
            LoveValue = int.Parse(parameters[4]);
        }
    }
}
