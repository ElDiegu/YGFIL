using System.Collections;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using YGFIL.Managers;

namespace Systems
{
    public class Typewriter : MonoBehaviour
    {
        [SerializeField] private string text;
        [SerializeField] private TextMeshProUGUI textObject;

        //Delay when starting to write
        [SerializeField] private float startingDelay;

        //Delay between letters
        [SerializeField] private float writingDelay;

        private int currentCharacter = 0;
        private Coroutine typewriterCoroutine;

        private void Update()
        {
            var input = InputManager.Instance.GetInput();

            if (input.LeftClick) EndWriting();
        }

        private void EndWriting()
        {
            StopCoroutine(typewriterCoroutine);
            textObject.text = text;
        }

        public void Write()
        {
            typewriterCoroutine = StartCoroutine(TypewriterEffect());
        }

        public IEnumerator TypewriterEffect()
        {
            StringBuilder stringBuilder = new StringBuilder();
            textObject.text = "";
            currentCharacter = 0;

            while (currentCharacter < text.Length)
            {
                stringBuilder.Append(text[currentCharacter]);
                textObject.text = stringBuilder.ToString();
                currentCharacter++;

                yield return new WaitForSeconds(writingDelay);
            }
            
            yield return null;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Typewriter))]
    public class TypewriterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var typewriter = (Typewriter)target;
            
            if (GUILayout.Button("Start Writing Text")) typewriter.Write();
        }
    }
#endif
}