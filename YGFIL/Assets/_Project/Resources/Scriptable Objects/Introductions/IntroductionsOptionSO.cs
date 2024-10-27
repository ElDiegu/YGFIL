using UnityEngine;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewOption", menuName = "You're Gonna Fall: In Love/Introductions/Option")]
    public class IntroductionsOptionSO : ScriptableObject
    {   
        [field: SerializeField]
        public int ImageIndex { get; set; }
        
        [field: SerializeField]
        public string Text { get; set; }

        [field: SerializeField]
        public Color TextColor { get; set; }

        [field: SerializeField]
        public int LoveValue { get; set; }
        
        public IntroductionsOptionSO(string[] parameters) 
        {
            ImageIndex = int.Parse(parameters[1]);
            
            Text = parameters[2];
            
            Color textColor = new Color();
            bool valid = ColorUtility.TryParseHtmlString("#" + parameters[3], out textColor);
            if (valid) TextColor = textColor;
            else TextColor = Color.black;
            
             LoveValue = int.Parse(parameters[4]);
        }
    }
}
