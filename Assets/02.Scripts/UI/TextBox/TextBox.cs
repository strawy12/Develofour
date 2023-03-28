using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using static Sound;
using System.Collections.Generic;
using ExtenstionMethod;

public class TextBox : MonoUI
{
    [SerializeField]
    private ContentSizeFitterText messageText;

    [SerializeField]
    private float printTextDelay = 0.05f;

    [SerializeField]
    private Image bgImage;

    [SerializeField]
    private Sprite simpleTypeSprite;

    [SerializeField]
    private Vector2 offsetSize;

    private float currentDelay = 0f;

    private TextData currentTextData;
    private string currentString;

    public bool isTextPrinting = false;
    private bool isActive = false;

    private Dictionary<int, Action> triggerDictionary;

    public void Init(TextData data, string str, Dictionary<int, Action> triggerList)
    {
        EndPrintText();
        currentTextData = data;
        currentString = str;
        triggerDictionary = triggerList;

        messageText.SetText("");

        //ShowBox();
        PrintText();
    }

    public void PrintText()
    {
        if (isTextPrinting) 
        {
            //if (CheckDataEnd())
            //{
            //    EndPrintText();
            //    return;
            //}
            Debug.Log("리턴");
            
            return; 
        }

        isTextPrinting = true;
        StartCoroutine(PrintMonologTextCoroutine());
    }

    public bool CheckDataEnd()
    {
        if (currentTextData == null)
        {
            GameManager.Inst.ChangeGameState(EGameState.Game);
            return true;
        }
        return messageText.text.Length >= currentString.Length ;
    }


    private IEnumerator PrintMonologTextCoroutine()
    {
        bool isRich = false;

        string msg = currentString;

        msg = SliceLineText(msg);

        SimpleTypePrint(currentTextData, msg);

        messageText.maxVisibleCharacters = 0;
        messageText.SetText(msg);


        for (int i = 0; i < msg.Length; i++)
        {
            if (triggerDictionary.ContainsKey(i))
            {
                triggerDictionary[i]?.Invoke();
                triggerDictionary[i] = null;
            }

            if (msg[i] == '<')
            {
                isRich = true;
            }

            if (msg[i] == '>')
            {
                isRich = false;
                continue;
            }

            if (!isRich)
            {
                Sound.OnPlaySound?.Invoke(EAudioType.MonologueTyping);
            }

            if (!isRich)
            {
                yield return new WaitForSeconds(printTextDelay);
            }

            if (currentDelay != 0f)
            {
                yield return new WaitForSeconds(currentDelay);
                currentDelay = 0f;
            }

            messageText.maxVisibleCharacters++;
        }

        isTextPrinting = false;
        Debug.Log(1);
    }

    public void ShowBox()
    {
        if (isActive) return;

        isActive = true;
        SetActive(true);
    }

    public void HideBox()
    {
        messageText.SetText("");
        isTextPrinting = false;
        isActive = false;
        SetActive(false);
    }

    public void EndPrintText()
    {
        StopAllCoroutines();
        //isTextPrinting = false;
        HideBox();
    }

    public void SimpleTypePrint(TextData data, string str)
    {
        bgImage.color = Color.black;
        bgImage.ChangeImageAlpha(1f);
        bgImage.sprite = simpleTypeSprite;

        messageText.SetText(str);
        ShowBox();
        bgImage.rectTransform.sizeDelta = messageText.rectTransform.sizeDelta + offsetSize;
        messageText.SetText("");
        messageText.color = data.color;
    }


    // 텍스트 길이가 길 시 자동 줄 바꿈
    private string SliceLineText(string text)
    {
        for (int i = 40; i < text.Length; i += 41)
        {
            text = text.Insert(i, "\n");
        }
        return text;
    }

    public void SetDelay(float value)
    {
        currentDelay = value;
    }
}