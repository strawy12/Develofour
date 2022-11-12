using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static Sound;

public class TextBox : MonoUI
{
    private int currentTextIndex;

    [SerializeField]
    private TMP_Text boxShowText;
    [SerializeField]
    private float printTextDuration = 0.05f;

    private TextDataSO currentTextData;

    private bool isTextPrinted = false;
    private bool isActive = false;

    private void Start()
    {
        EventManager.StartListening(EEvent.OpenTextBox, Init);
    }

    private void Update()
    {
        if (GameManager.Inst.GameState != EGameState.UI) return;
        if (isActive == false) return;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTextPrinted)
            {
                SkipPrintText();
            }
            else
            {
                PrintText();
            }
        }
    }

    private void Init(object param)
    {
        Debug.Log(2);
        if (param == null || !(param is ETextDataType))
        {
            return;
        }


        ETextDataType textDataType = (ETextDataType)param;

        GameManager.Inst.ChangeGameState(EGameState.UI);

        currentTextData = GetTextData(textDataType);
        currentTextIndex = 0;

        ShowBox();
    }

    public void ShowBox()
    {
        isActive = true;
        SetActive(true);
        PrintText();
    }

    public void HideBox()
    {
        isActive = false;
        SetActive(false);
    }

    public void PrintText()
    {
        if (isTextPrinted) { return; }
        if (CheckDataEnd())
        {
            EndPrintText();
            return;
        }

        isTextPrinted = true;
        StartCoroutine(PrintTextCoroutine(currentTextData[currentTextIndex++]));
    }

    private IEnumerator PrintTextCoroutine(string message)
    {
        message = message.Replace("\r", "");
        boxShowText.text = "";
        string text = "";
        for (int i = 0; i < message.Length; i++)
        {
            if (!isTextPrinted) { break; }

            char c = message[i];

            if (c == '{')
            {
                int cnt = CommandTrigger(message.Substring(i));
                i += cnt;
                c = message[i];
            }

            text = string.Format("{0}{1}", text, c);
            boxShowText.SetText(text);
            yield return new WaitForSeconds(printTextDuration);
        }

        if (isTextPrinted == false)
        {
            CompleteText(message);
        }

        isTextPrinted = false;
    }

    private void CompleteText(string msg)
    {
        bool isCmdMsg = false;
        string completeMsg = "";
        foreach (char c in msg)
        {
            if (isCmdMsg)
            {
                if (c == '}')
                {
                    isCmdMsg = false;
                }
                continue;
            }

            if (c == '{')
            {
                isCmdMsg = true;
                continue;
            }

            completeMsg = $"{completeMsg}{c}";
        }

        boxShowText.SetText(completeMsg);
    }

    public TextDataSO GetTextData(ETextDataType textDataType)
    {
        TextDataSO textDataSO = null;
        try
        {
            textDataSO = Resources.Load<TextDataSO>($"TextData/TextData_{textDataType}");
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
        if (currentTextIndex < currentTextData.Count)
        {
            return false;
        }
        return true;
    }

    private void SkipPrintText()
    {
        if (!isTextPrinted) { return; }
        isTextPrinted = false;
    }
    public void EndPrintText()
    {
        HideBox();
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
                    Sound.EEffect effectType = (EEffect)Enum.Parse(typeof(EEffect), cmdValue);
                    Sound.OnPlayEffectSound(effectType);
                    break;
                }
            case "BS":
                Sound.EBgm bgmType = (EBgm)Enum.Parse(typeof(EBgm), cmdValue);
                Sound.OnPlayBGMSound(bgmType);
                break;
        }

        return cnt;
    }

}
