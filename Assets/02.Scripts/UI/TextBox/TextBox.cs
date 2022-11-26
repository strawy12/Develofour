using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using static Sound;
using System.Collections.Generic;

public class TextBox : MonoUI
{
    private int currentTextIndex;

    [SerializeField]
    private TMP_Text messageText;

    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private float printTextDelay = 0.05f;

    private TextDataSO currentTextData;

    private bool isTextPrinted = false;
    private bool isActive = false;
    private bool isEffected = false;
    private bool isFindSign = false;

    private Dictionary<int, Action> triggerDictionary;

    private void Start()
    {
        triggerDictionary = new Dictionary<int, Action>();

        EventManager.StartListening(ECoreEvent.OpenTextBox, Init);
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

    private void Init(object[] param)
    {
        if (param == null || !(param[0] is ETextDataType))
        {
            return;
        }

        Init((ETextDataType)param[0]);
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
        TextData textData = currentTextData[currentTextIndex++];
        
        nameText.SetText(textData.name);

        StartCoroutine(PrintTextCoroutine(textData.text));

        return textData.text.Length * printTextDelay;
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

    private string EncordingCommandText(string message)
    {
        string richText = "";

        for (int i = 0; i < message.Length; i++)
        {
            if (message[i] == '}')
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
        messageText.text = "";

        string originalText;
        string removeSignText;
        string textBoxInText;

        originalText = message;

        removeSignText = RemoveColor(message);
        removeSignText = RemoveCommand(removeSignText);

        textBoxInText = RemoveCommand(message);

        bool isRich = false;
        
        messageText.text = textBoxInText;

        for (int i = 0; i < textBoxInText.Length; i++)
        {
            if (message[i] == '<')
            {
                isRich = true;
            }

            if (message[i] == '>')
            {
                isRich = false;
                continue;
            }

            messageText.maxVisibleCharacters = i;
            if(triggerDictionary.ContainsKey(i) && isFindSign)
            {
                isFindSign = false;
                triggerDictionary[i]?.Invoke();
            }
            

            if(!isRich)
            {
                yield return new WaitForSeconds(printTextDelay);
            }
        }

        if (isTextPrinted == false)
        {
            CompleteText(message);
        }

        isTextPrinted = false;
    }

    private string RemoveColor(string message)
    {
        string removeText = message;

        for (int i = 0; i < removeText.Length; i++)
        {
            if (removeText[i] == '<')
            {
                string signText = EncordingRichText(removeText.Substring(i)); // < color > 문자열
                
                removeText = removeText.Remove(i, signText.Length); // < > 이 문자열을 제외시킨 문자열
               
                i -= signText.Length;
            }
        }

        return removeText;
    }

    private string RemoveCommand(string message)
    {
        string removeText = message;

        for (int i = 0; i < removeText.Length; i++)
        {
            if (removeText[i] == '{')
            {
                isFindSign = true;

                string signText = EncordingCommandText(removeText.Substring(i)); // {} 문자열
                
                triggerDictionary.Add(i, () => CommandTrigger(signText));
                                                                                                                                                                                                              
                removeText = removeText.Remove(i, signText.Length); // {} 이 문자열을 제외시킨 문자열

                i -= signText.Length;
            }
        }

        return removeText;
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

        messageText.SetText(completeMsg);
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
            Debug.Log($"TextData{textDataType} is null\n{e}");
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
         Debug.Log(msg);
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
        messageText.rectTransform.DOShakeAnchorPos(delay, strength, vibrato, 0, true);
        yield return new WaitForSeconds(delay);
        isEffected = false;
    }
}
