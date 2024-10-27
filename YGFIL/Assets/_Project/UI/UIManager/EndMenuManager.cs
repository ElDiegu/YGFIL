using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YGFIL.Managers;
using YGFIL.Monsters;

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
            AudioManager.Instance.Play("showMusic");

            loadingOperation.allowSceneActivation = true;
        }

        public void setEnd(MonsterType type)
        {
            switch (type)
            {
                case MonsterType.Zombie:
                    backgroundImage.sprite = ends[0];
                    break;
                case MonsterType.Medusa:
                    backgroundImage.sprite = ends[1];
                    break;
                case MonsterType.Vampire:
                    backgroundImage.sprite = ends[2];
                    break;
                case MonsterType.Spider:
                    backgroundImage.sprite = ends[3];
                    break;
                case MonsterType.Wolf:
                    backgroundImage.sprite = ends[4];
                    break;
                case MonsterType.Wolf2:
                    backgroundImage.sprite = ends[4];
                    break;
                case MonsterType.Succubus:
                    backgroundImage.sprite = ends[5];
                    break;
            }
        }
    }
}
