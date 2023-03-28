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
    private Vector2 offsetSize;

    private float currentDelay = 0f;

    private TextData currentTextData;

    private bool isTextPrinting = false;
    private bool isActive = false;

    private Dictionary<int, Action> triggerDictionary;

    public void Init(TextData data, Dictionary<int, Action> triggerList)
    {
        currentTextData = data;
        triggerDictionary = triggerList;

        messageText.SetText("");

        ShowBox();
        PrintText();
    }

    public void PrintText()
    {
        if (isTextPrinting) { return; }

        isTextPrinting = true;

        StartCoroutine(PrintMonologTextCoroutine());
    }

    private IEnumerator PrintMonologTextCoroutine()
    {
        bool isRich = false;

        string msg = currentTextData.text;

        msg = SliceLineText(msg);

        messageText.SetText(msg);
        messageText.maxVisibleCharacters = 0;

        for (int i = 0; i < msg.Length; i++)
        {

            if (msg[i] == '<')
            {
                isRich = true;
            }

            if (msg[i] == '>')
            {
                isRich = false;
                continue;
            }

            messageText.maxVisibleCharacters++;

            if (!isRich)
            {
                Sound.OnPlaySound?.Invoke(EAudioType.MonologueTyping);
            }

            if (triggerDictionary.ContainsKey(i))
            {
                triggerDictionary[i]?.Invoke();
            }

            // Rich 일때는 한번에 나오게 하기 위해서 딜레이 X
            if (!isRich)
            {
                yield return new WaitForSeconds(printTextDelay);
            }

            if (currentDelay != 0f)
            {
                yield return new WaitForSeconds(currentDelay);
                currentDelay = 0f;
            }
        }

        HideBox();
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

        isActive = false;
        SetActive(false);

    }

    public void EndPrintText()
    {
        StopAllCoroutines();
        HideBox();
        isTextPrinting = false;
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