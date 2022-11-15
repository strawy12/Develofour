using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using static Sound;


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
    private bool isEffected = false;

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

    private string EncordingRichText(string message)
    {
        string richText = "";

        for (int i = 0; i < message.Length; i++)
        {
            if (message[i] == '>')
            {
                richText += message[i];
                break;
            }

            richText += message[i];
        }

        return richText;
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
                string richText = EncordingRichText(message.Substring(i));

                text = $"{text}{richText}";
                i += richText.Length - 1;
                continue;
            }

            if (c == '{')
            {
                int cnt = CommandTrigger(message.Substring(i));
                i += cnt;

                if (i >= message.Length)
                    break;

                c = message[i];
            }
            yield return new WaitUntil(() => isEffected == false);

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
                {
                    Sound.EBgm bgmType = (EBgm)Enum.Parse(typeof(EBgm), cmdValue);
                    Sound.OnPlayBGMSound(bgmType);
                    break;
                }
            case "SK":
                {
                    string[] cmdValueArray = cmdValue.Split(',');
                    string cmdValue1 = cmdValueArray[0];
                    string cmdValue2 = cmdValueArray[1];
                    string cmdValue3 = cmdValueArray[2];
                    float delay = float.Parse(cmdValue1);
                    float strength = float.Parse(cmdValue2);
                    int vibrato = int.Parse(cmdValue3);

                    StartCoroutine(textShakingCoroutine(delay, strength, vibrato));
                    break;
                }
        }

        return cnt;
    }

    private IEnumerator textShakingCoroutine(float delay, float strength, int vibrato)
    {
        isEffected = true;
        boxShowText.rectTransform.DOShakeAnchorPos(delay, strength, vibrato, 0, true);
        yield return new WaitForSeconds(delay);
        isEffected = false;
    }
}
