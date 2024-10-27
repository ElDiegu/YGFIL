using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YGFIL.Managers;

namespace YGFIL.Managers
{
    public class EndMenuManager : StaticInstance<EndMenuManager>
    {
        [SerializeField] private Animator fadeOutAnimator;
        [SerializeField] private List<Sprite> ends;
        [SerializeField] private SpriteRenderer backgroundImage;

        private void Update()
        {
            var input = InputManager.Instance.GetInput();

            if (input.LeftClick)
            {
                StartCoroutine(StartPlayingCoroutine());
            }
        }

        private IEnumerator StartPlayingCoroutine()
        {
            fadeOutAnimator.Play("FadeOutImage");

            yield return new WaitForSeconds(0.001f);

            while (fadeOutAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return null;

            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("MonsterSelection");

            loadingOperation.allowSceneActivation = false;

            while (loadingOperation.progress < 0.9f) yield return null;

            loadingOperation.allowSceneActivation = true;
        }

        public void setEnd(int monsterIndex)
        {
            backgroundImage.sprite = ends[monsterIndex];
        }
    }
}
