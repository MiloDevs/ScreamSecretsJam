using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueContent : ScriptableObject
{
    public string dialogueName;

    [TextArea]
    public string[] dialogueLines;
    public Sprite sprite;
    public string nextDialogue;
    public bool endOfSequence;
   


}
