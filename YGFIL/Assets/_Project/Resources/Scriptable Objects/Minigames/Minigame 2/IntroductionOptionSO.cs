using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewOption", menuName = "You're Gonna Fall: In Love/Minigames/Minigame 2 Option")]
    public class IntroductionOptionSO : ScriptableObject
    {
        [field: SerializeField]
        public Image Image { get; set; }

        [field: SerializeField]
        public int LoveValue { get; set; }
    }
}
