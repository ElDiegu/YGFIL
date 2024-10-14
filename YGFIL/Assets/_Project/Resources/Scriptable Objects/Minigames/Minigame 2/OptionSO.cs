using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewOption", menuName = "You're Gonna Fall: In Love/Minigames/Minigame Two Option")]
    public class OptionSO : ScriptableObject
    {
        [field: SerializeField]
        public Image Image { get; set; }

        [field: SerializeField]
        public float LoveValue { get; set; }
    }
}
