using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class DialogueName : DialogueText
{
    public override void SetText(string t)
    {
        if(t == "")
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
            base.SetText(t);
            StartCoroutine(WaitFrameSetText());
        }
    }
    IEnumerator WaitFrameSetText()
    {
        yield return new WaitForEndOfFrame();
        RectTransform transform = GetComponent<RectTransform>();
        RectTransform textTransform = text.GetComponent<RectTransform>();
        transform.sizeDelta = new Vector2(textTransform.sizeDelta.x + margin, transform.sizeDelta.y);

        //GetComponent<SetOutlineSize>().UpdateOutlineSize();
    }
}
