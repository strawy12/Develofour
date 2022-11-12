using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TextBox : MonoUI
{
    private int currentTextIndex;

    [SerializeField]
    private TMP_Text boxShowText;
    [SerializeField]
    private TextDataSO currentTextData;

    private bool isTextPrinted = false;
    private void Start()
    {
        EventManager.StartListening(EEvent.OpenTextBox, Init);
    }

    private void Update()
    {
        if (GameManager.Inst.GameState != EGameState.UI) return;
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (CheckDataEnd())
            {
                SkipTextEffect();
            }
            else
            {
                ShowBox();
            }
        }
    }

   

    private void Init(object param)
    {
        if(param == null || !(param is ETextDataType))
        {
            return;
        }

        ETextDataType textDataType = (ETextDataType)param;

        GameManager.Inst.ChangeGameState(EGameState.UI);

        currentTextData = GetTextData(textDataType);
        currentTextIndex = 0;
    }

    public void ShowBox()
    {
        SetActive(true);
        PrintText();
    }

    public void HideBox()
    {
        SetActive(false);
    }

    public void PrintText()
    {
        if(CheckDataEnd())
        {
            return;
        }

        boxShowText.SetText(currentTextData[currentTextIndex]);
        currentTextIndex++;
    }

    public TextDataSO GetTextData(ETextDataType textDataType)
    {
        TextDataSO textDataSO = null;
        try
        {
            textDataSO = Resources.Load($"Resources/TextData/TextData{textDataType}") as TextDataSO;
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log($"TextData{textDataType} is null");
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
        return textDataSO;
    }

    public bool CheckDataEnd()
    {
        if(currentTextIndex < currentTextData.Count)
        {
            return false;
        }
        return true;
    }

    private void SkipTextEffect()
    {

    }

    public void EndTextBox()
    {

        HideBox();
    }
}
