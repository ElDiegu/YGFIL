using Microsoft.Unity.VisualStudio.Editor;
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
    }
}
