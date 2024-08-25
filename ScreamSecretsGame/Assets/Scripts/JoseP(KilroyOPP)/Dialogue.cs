using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    
    [SerializeField] GameObject textGameObject;
    [SerializeField] DialogueContent[] content;
    [SerializeField] float textSpeed;
    [SerializeField] float textPauseSpeed;
    [SerializeField] string playThisDialogue;
    [SerializeField] char[] pauses;
    
    

    int contentIndex;
    TextMeshProUGUI textComponent;
    Image imageComponent;

    private int index;
   

    void Start()
    {
        textGameObject.SetActive(false);

        textComponent = textGameObject.GetComponent<TextMeshProUGUI>();
        

        textComponent.text = string.Empty;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
        { 
            loadFullText();
        }
    }

    public void StartDialogueOnInteraction(string dialogueName)
    {
        if (textGameObject == true)
        {
            loadFullText();
        }
        textGameObject.SetActive(true);
        SetNewDialogueContent(dialogueName);
        StartDialogue();
    }

    public void loadFullText()
    {
        if (textComponent.text == content[contentIndex].dialogueLines[index].ToString())
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = content[contentIndex].dialogueLines[index];
        }
    }
    public void SetNewDialogueContent(string dialogueName)
    {

        //contentIndex = Array.FindIndex(content, 0, 1, dialogueName);
        for (int i = 0; i < content.Length; i++)
        {
            if (dialogueName == content[i].name)
            {

                contentIndex = i; break;
            }
        }
    }

    public void StartDialogue()
    {
        textComponent.text = "";
        index = 0;
        StartCoroutine(TypeLine());

    }

    IEnumerator TypeLine()
    {
        foreach (char c in content[contentIndex].dialogueLines[index].ToCharArray())
        {
            
            for (int i = 0; i < pauses.Length; i++)
            {
                if (c == pauses[i])
                {
                    yield return new WaitForSeconds(textPauseSpeed);
                }
            }
            textComponent.text += c;
           
            yield return new WaitForSeconds(textSpeed);
        }
        
    }

    void NextLine()
    {
        if (index < content[contentIndex].dialogueLines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            textGameObject.SetActive(false);
        }
    }

    
    
}
