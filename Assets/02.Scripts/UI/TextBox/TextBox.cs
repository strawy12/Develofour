using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Sound;

public class TextBox : MonoUI
{
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
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            ShowBox();
        }
    }

    private void Init(object param)
    {
        if (param == null || !(param is ETextDataType))
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
        if (currentTextIndex >= currentTextData.Count)
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

    private int CommandTrigger(string msg)
    {
        string cmdMsg = "";
        msg = msg.Substring(1);
        int cnt = 1;
        foreach (char c in msg)
        {
            cnt++;
            if (c == '}')
            {
                break;
            }
            else
            {
                cmdMsg = $"{cmdMsg}{c}";
            }
        }

        string[] cmdMsgSplit = cmdMsg.Split('_');
        string cmdType = cmdMsgSplit[0];
        string cmdValue = cmdMsgSplit[1];

        switch (cmdType)
        {
            case "ES":
            {
                Sound.EEffect effectType = (EEffect)System.Enum.Parse(typeof(EEffect), cmdValue);
                Sound.OnPlayEffectSound(effectType);
                break;
             }
            case "BS":
                Sound.EBgm bgmType = (EBgm)System.Enum.Parse(typeof(EBgm), cmdValue);
                Sound.OnPlayBGMSound(bgmType);
                break;
        }

        return cnt;
    }
}
