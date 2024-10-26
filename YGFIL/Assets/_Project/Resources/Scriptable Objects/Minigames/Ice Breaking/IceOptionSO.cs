using System.Collections.Generic;
using UnityEngine;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(menuName = "You're Gonna Fall: In Love/Ice Breaking/Option")]
    public class IceOptionSO : ScriptableObject 
    {
        [field: SerializeField]
        public List<Sprite> Images { get; private set; }
        
        [field: SerializeField]
        public int NoteType { get; private set; }
        
        [field: SerializeField]
        public string Text { get; private set; }

        [field: SerializeField]
        public int LoveValue { get; private set; }
    }
}
