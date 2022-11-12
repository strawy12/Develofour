using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TextBox : MonoUI
{
    private bool isTextBox;
    private int currentTextIndex;

    [SerializeField]
    private TMP_Text boxShowText;

    [SerializeField]
    private TextDataSO currentTextData;

    private void Start()
    {
        EventManager.StartListening(EEvent.OpenTextBox, Init);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if(isTextBox)
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
        isTextBox = true;

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
        isTextBox = false;
        SetActive(false);
    }

    public void PrintText()
    {
        if(currentTextIndex >= currentTextData.Count)
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
}
