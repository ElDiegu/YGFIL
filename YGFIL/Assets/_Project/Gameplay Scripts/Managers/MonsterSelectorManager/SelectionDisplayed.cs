using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YGFIL.Monsters;

namespace YGFIL
{
    public class SelectionDisplayed : MonoBehaviour
    {
        [SerializeField] public MonsterSelectionSO monsterSelectionSO;
        [SerializeField] Image faceImage;
        [SerializeField] TMP_Text nameText;
        [SerializeField] Image selectorImage;
        [SerializeField] GameObject locker;
        [SerializeField] public GameObject completed;
        [SerializeField] public Button button;

        void Start()
        {
            selectorImage.sprite = monsterSelectionSO.SelectorImage;
            faceImage.sprite = monsterSelectionSO.FaceImage;
            nameText.text = monsterSelectionSO.Name;
            if (MonsterSelectorManager.Instance.datesCompleted < monsterSelectionSO.datesCompletedNeeded)
            {
                int datesLeft = monsterSelectionSO.datesCompletedNeeded - MonsterSelectorManager.Instance.datesCompleted;
                string s = (datesLeft > 1) ? "s" : "";
                locker.SetActive(true);
                button.interactable = false;
                nameText.text = "Completa " + datesLeft + " cita" + s + " más para desbloquear";
            }
        }

        public void checkUnlockSelection()
        {
            if (MonsterSelectorManager.Instance.datesCompleted >= monsterSelectionSO.datesCompletedNeeded)
            {
                locker.SetActive(false);
                nameText.text = monsterSelectionSO.Name;
                button.interactable = true;
            }
        }
    }
}
