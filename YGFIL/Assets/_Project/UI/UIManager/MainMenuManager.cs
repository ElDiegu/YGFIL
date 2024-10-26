using System.Collections;
using UnityEngine;

namespace YGFIL.Managers
{
    public class MainMenuManager : StaticInstance<MainMenuManager>
    {
        [SerializeField] private Animator titleAnimator;
        [SerializeField] private Animator fadeOutAnimator;
        
        private void Update()
        {
            var input = InputManager.Instance.GetInput();
            
            if (input.LeftClick && titleAnimator.GetCurrentAnimatorStateInfo(0).IsName("title_loop")) 
            {
                StartCoroutine(StartPlayingCoroutine());
            }
        }
        
        private IEnumerator StartPlayingCoroutine() 
        {
            titleAnimator.SetTrigger("TitleLeave");
            
            while (titleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            fadeOutAnimator.Play("FadeOut");
            
            while (fadeOutAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
        }
    }
}
