using Systems.EventSystem;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace YGFIL.Managers
{
    public class UIManager : StaticInstance<UIManager>
    {
        [field: SerializeField]
        public Animator UIAnimator { get; private set; }
        
        EventBinding<PlayAnimationEvent> playAnimationBinding;
        
        private void OnEnable()
        {
            playAnimationBinding = new EventBinding<PlayAnimationEvent>(playAnimationEvent => UIAnimator.Play(playAnimationEvent.animationString));
            EventBus<PlayAnimationEvent>.Register(playAnimationBinding);
        }
        
        private void OnDisable()
        {
            EventBus<PlayAnimationEvent>.Deregister(playAnimationBinding);
        }
    }
    
    public struct PlayAnimationEvent : IEvent 
    {
        public string animationString;
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(UIManager))]
    public class UIManagerEditor : Editor 
    {
        public static string animationName;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            animationName = EditorGUILayout.TextField(animationName);
            
            if (GUILayout.Button("Play Animation")) 
            {
                Debug.Log($"Trying to play {animationName}");
                
                EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent() 
                {
                    animationString = animationName
                });
            }
        }
    }
#endif
}
