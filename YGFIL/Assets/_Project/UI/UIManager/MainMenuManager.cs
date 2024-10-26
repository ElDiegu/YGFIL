using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using YGFIL.Utils;

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
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("DateScene");
            
            loadingOperation.allowSceneActivation = false;
            
            titleAnimator.SetTrigger("TitleLeave");
            
            while (titleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            fadeOutAnimator.Play("FadeOutImage");
            
            while (fadeOutAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;
            
            while (loadingOperation.progress < 0.9f) yield return null; 
            
            loadingOperation.allowSceneActivation = true;
        }
    }
}
