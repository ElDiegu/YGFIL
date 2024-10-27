using System.Collections.Generic;
using UnityEngine;

namespace YGFIL.ScriptableObjects
{
    [CreateAssetMenu(menuName = "You're Gonna Fall: In Love/Ice Breaking/Option")]
    public class IceOptionSO : ScriptableObject 
    {
        [field: SerializeField]
        public int NoteType { get; private set; }
        
        [field: SerializeField]
        public string Text { get; private set; }

        [field: SerializeField]
        public int LoveValue { get; private set; }
        
        public IceOptionSO(string[] parameters) 
        {
            NoteType = int.Parse(parameters[1]);
            Text = parameters[2];
            LoveValue = int.Parse(parameters[3]);
        }
    }
}
