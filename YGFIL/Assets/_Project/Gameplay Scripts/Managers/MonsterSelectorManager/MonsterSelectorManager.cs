using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YGFIL.Managers;
using YGFIL.Monsters;

namespace YGFIL
{
    public class MonsterSelectorManager : StaticInstance<MonsterSelectorManager>
    {
        [SerializeField] private List<SelectionDisplayed> monstersSelections;

        public int datesCompleted = 0;

        void Start()
        {
            foreach (MonsterType monster in GameManager.Instance.completedMonsters)
            {
                DateCompleted(monster);
            }
        }

        public void DateCompleted(MonsterType monster)
        {
            datesCompleted++;
            foreach (SelectionDisplayed selection in monstersSelections)
            {
                selection.checkUnlockSelection();
                if (selection.monsterSelectionSO.MonsterType == monster.ToString())
                {
                    selection.completed.SetActive(true);
                    selection.button.interactable = false;
                }
            }
        }  

        public void StartDate(int monsterType)
        {
            AudioManager.Instance.Play("button");
            AudioManager.Instance.Stop("showMusic");
            AudioManager.Instance.Play("dateMusic");
            GameManager.Instance.setMonster(monsterType);
            SceneManager.LoadSceneAsync("DateScene");
        }
    }
}
