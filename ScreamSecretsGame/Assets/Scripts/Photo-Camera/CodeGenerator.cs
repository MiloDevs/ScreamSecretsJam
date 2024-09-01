using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;



public class CodeGenerator : MonoBehaviour
{
    private TextMeshProUGUI[] codeTexts;

    public int code;


    private void Start()
    {
        GameObject[] paintObjects = GameObject.FindGameObjectsWithTag("Paint");

        codeTexts = new TextMeshProUGUI[paintObjects.Length];

        for (int i = 0; i < paintObjects.Length; i++)
        {
            TextMeshProUGUI textComponent = paintObjects[i].GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                codeTexts[i] = textComponent;
            }
        }

        code = GenerateFourDigitNumber();
        CodeImplementation();
    }


    public static int GenerateFourDigitNumber()
    {
        return Random.Range(1000, 10000);
    }


    void CodeImplementation()
    {
        string codeString = code.ToString("D4");
        if (codeTexts.Length >= 4)
        {
            for (int i = 0; i < 4; i++)
            {
                codeTexts[i].text = codeString[i].ToString();
            }
        }
    }



}
