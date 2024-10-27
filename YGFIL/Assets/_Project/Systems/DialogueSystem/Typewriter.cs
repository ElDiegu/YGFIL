using System.Collections;
using System.Text;
using Systems.EventSystem;
using TMPro;
using UnityEditor;
using UnityEngine;
using YGFIL;
using YGFIL.Managers;

namespace YGFIL.Systems
{
    public class Typewriter : MonoBehaviour
    {
        [SerializeField] private DialogData dialog;
        [SerializeField] private TextMeshProUGUI monsterText, casterText;

        //Delay when starting to write
        [SerializeField] private float startingDelay;

        //Delay between letters
        [SerializeField] private float writingDelay;

        private int currentChar = 0;
        private Coroutine typewriterCoroutine;
        private string text;

        public void Write(DialogData dialog) 
        {
            this.dialog = dialog;
            text = dialog.Text;
            Write();
        }
        
        public void Write()
        {
            typewriterCoroutine = StartCoroutine(TypewriterEffect());
            
            EventBus<OnTypewriterStartEvent>.Raise(new OnTypewriterStartEvent());
        }

        public void EndWriting()
        {
            if (!IsWriting()) return;
            
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
            
            var writingText = (int)dialog.Monster == 1 ? casterText : monsterText;
            writingText.text = dialog.Text;
            
            EventBus<OnTypweWriterFinishEvent>.Raise(new OnTypweWriterFinishEvent() 
            {
                isEnd = dialog.Tag.Contains("End"),
                nextID = dialog.NextID
            });
        }

        public IEnumerator TypewriterEffect()
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            monsterText.text = "";
            casterText.text = "";
            
            TextMeshProUGUI writingText;
            
            if ((int)dialog.Monster == 1) 
            {
                writingText = casterText;
                casterText.transform.parent.gameObject.SetActive(true);
            }
            else 
            {
                writingText = monsterText;
                casterText.transform.parent.gameObject.SetActive(false);
            }
            
            currentChar = 0;

            while (currentChar < text.Length)
            {
                stringBuilder.Append(text[currentChar]);
                writingText.text = stringBuilder.ToString();
                currentChar++;

                yield return new WaitForSeconds(writingDelay);
            }
            
            EndWriting();
            
            yield return null;
        }
        
        public bool IsWriting() => typewriterCoroutine != null;
    }
    
    public struct OnTypweWriterFinishEvent : IEvent 
    {
        public bool isEnd;
        public int nextID;
    }
    
    public struct OnTypewriterStartEvent : IEvent 
    {
        
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