using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox : DialogueText
{
    [SerializeField] public DialogueManager dialogueManager;
    private bool writingText = false;
    //private bool optional = false;
    public void SetTextInTime(string t)
    {
        writingText = true;
        StartCoroutine(SetTextInTimeIEnumerator(t));
    }
    IEnumerator SetTextInTimeIEnumerator(string t)
    {
        for(int i = 0; i < t.Length; i++)
        {
            if (!writingText)
            {
                text.text = t;
                /*if (optional)
                {
                    ShowOptions();
                }*/
                yield break;
            }
            text.text = t.Substring(0, i + 1);
            yield return new WaitForSeconds(0.02f);
        }
        writingText = false;
        /*if (optional)
        {
            ShowOptions();
        }*/
    }
    public void SetWritingText(bool b)
    {
        writingText = b;
    }
    /*public void SetOptional(bool b)
    {
        optional = b;
    }*/
    public bool GetWritingText()
    {
        return writingText;
    }
    /*public bool GetOptional()
    {
        return optional;
    }
    private void ShowOptions()
    {
        dialogueManager.ShowOptions();
    }*/
    public override void SetText(string t)
    {
        base.SetText(t);
        writingText = false;
    }
}
