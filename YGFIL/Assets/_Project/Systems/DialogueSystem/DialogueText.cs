using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DialogueText : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected float margin = 0f; 
    public virtual void SetText(string t)
    {
        text.text = t;
    }
    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
        text.text = "";
    }
    public virtual void Show()
    {
        this.gameObject.SetActive(true);
    }
    public TextMeshProUGUI GetTextMeshPro()
    {
        return text;
    } 
}
