using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YGFIL.Managers;
using YGFIL.Monsters;

namespace YGFIL
{
    public class MonsterSelectorManager : StaticInstance<MonsterSelectorManager>
    {
        [SerializeField] private List<SelectionDisplayed> monstersSelections;

        public int datesCompleted = 0;

        public void DateCompleted(Monster monster)
        {
            datesCompleted++;
            foreach (SelectionDisplayed selection in monstersSelections)
            {
                selection.checkUnlockSelection();
                if (selection.monsterSelectionSO.MonsterType == monster.monsterType.ToString())
                {
                    selection.completed.SetActive(true);
                    selection.button.interactable = false;
                }
            }
        }  

        public void StartDate(string monsterType)
        {
            AudioManager.instance.Play("button");
            //change scene
        }
    }
}
