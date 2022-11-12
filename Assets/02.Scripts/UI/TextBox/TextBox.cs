using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static Sound;
using UnityEditor.Experimental.GraphView;

public class TextBox : MonoUI
{
    private int currentTextIndex;

    [SerializeField]
    private TMP_Text boxShowText;
    [SerializeField]
    private float printTextDelay = 0.05f;

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
        if (param == null || !(param is ETextDataType))
        {
            return;
        }

        Init((ETextDataType)param);
        ShowBox();
        PrintText();
    }

    public void Init(ETextDataType textDataType)
    {
        if (GameManager.Inst.GameState != EGameState.CutScene)
        {
            GameManager.Inst.ChangeGameState(EGameState.UI);
        }

        currentTextData = GetTextData(textDataType);
        currentTextIndex = 0;
    }

    public void ShowBox()
    {
        if (isActive) return;

        isActive = true;
        SetActive(true);
    }

    public void HideBox()
    {
        isActive = false;
        SetActive(false);
    }

    public float PrintText()
    {
        if (isTextPrinted) { return 0f; }
        if (CheckDataEnd())
        {
            EndPrintText();
            return 0f;
        }

        isTextPrinted = true;
        string text = currentTextData[currentTextIndex++];
        StartCoroutine(PrintTextCoroutine(text));

        return text.Length * printTextDelay;
    }

    private string PaintedText(string message)
    {
        int secondBracket = 0;

        for (int i = 0; i < message.Length; i++)
        {
            if (message[i] == '>' && secondBracket >= 1)
            {
                return message.Substring(0, i + 1);
            }
            else if (message[i] == '>')
            {
                secondBracket++;
            }
        }

        return null;
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

            if (c == '<')
            {
                string changeText = PaintedText(message.Substring(i));

                text = string.Format("{0}{1}", text, changeText);
                i += changeText.Length - 1;

                boxShowText.SetText(text);

                continue;
            }

            if (c == '{')
            {
                int cnt = CommandTrigger(message.Substring(i));
                i += cnt;
                c = message[i];
            }

            text = string.Format("{0}{1}", text, c);
            boxShowText.SetText(text);
            yield return new WaitForSeconds(printTextDelay);
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
        StopAllCoroutines();
        HideBox();
        isTextPrinted = false;
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
