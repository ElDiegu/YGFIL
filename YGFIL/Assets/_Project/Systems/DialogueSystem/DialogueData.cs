using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public string id;
    public string speaker;
    public string text;
    public string expression;
    public string type;
    public string nextIndex;
    public DialogueData(string id, string speaker, string text, string expression, string type, string nextIndex)
    {
        this.id = id;
        this.speaker = speaker;
        this.text = text;
        this.expression = expression;
        this.type = type;
        this.nextIndex = nextIndex;
    }
}
