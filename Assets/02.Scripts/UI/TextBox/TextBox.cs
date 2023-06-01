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

    private string currentText;
    private Color currentColor;
    public bool isTextPrinting = false;
    private bool isActive = false;
    private bool isSkip;

    private Dictionary<int, Action> triggerDictionary;

    private void Start()
    {
        triggerDictionary = new Dictionary<int, Action>();
    }


    public void DictionaryClear()
    {
        triggerDictionary.Clear();
    }

    public void Init(string data, Dictionary<int, Action> triggerList, Color color)
    {
        EndPrintText();
        currentText = data;
        triggerDictionary = triggerList;

        messageText.SetText("");
        messageText.color = color;
        currentColor = color;
        //ShowBox();
        InputManager.Inst.AddMouseInput(EMouseType.LeftClick, onKeyDown: ImmediatelyComplete);
        PrintText();
    }

    private void ImmediatelyComplete()
    {
        if(!isSkip)
        {
            isSkip = true;
            return;
        }
        StopCoroutine(PrintMonologTextCoroutine());
        for(int i = 0; i < currentText.Length; i++)
        {
            if (triggerDictionary.ContainsKey(i))
            {
                triggerDictionary[i]?.Invoke();
                triggerDictionary[i] = null;
            }
        }
        messageText.maxVisibleCharacters = currentText.Length;
        string msg = currentText;
        msg = SliceLineText(msg);
        messageText.SetText(msg);
        currentDelay = 0;
        EndSetting();
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
        if (currentText == null)
        {
            GameManager.Inst.ChangeGameState(EGameState.Game);
            return true;
        }
        return messageText.text.Length >= currentText.Length;
    }

    private IEnumerator PrintMonologTextCoroutine()
    {
        bool isRich = false;

        string msg = currentText;

        msg = SliceLineText(msg);

        SimpleTypePrint(msg);

        messageText.maxVisibleCharacters = 0;
        messageText.SetText(msg);


        for (int i = 0; i < msg.Length; i++)
        {
            if (msg.Length - 1 == i)
            {
                messageText.maxVisibleCharacters++;
            }
            if (triggerDictionary.ContainsKey(i))
            {
                triggerDictionary[i]?.Invoke();
                triggerDictionary[i] = null;
            }

            if(!bgImage.enabled)
            {
                bgImage.enabled = true;
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

            if (currentDelay != 0f)
            {
                yield return new WaitForSeconds(currentDelay);
                currentDelay = 0f;
            }

            if (!isRich)
            {
                yield return new WaitForSeconds(printTextDelay);
            }

            if (msg.Length - 1 != i)
            {
                messageText.maxVisibleCharacters++;
            }
        }

        EndSetting();
    }

    private void EndSetting()
    {
        isTextPrinting = false;
        Sound.OnImmediatelyStop?.Invoke(EAudioType.MonologueTyping);
        isSkip = false;
        InputManager.Inst.RemoveMouseInput(EMouseType.LeftClick, onKeyDown: ImmediatelyComplete);
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
        messageText.color = Color.white;
        currentColor = Color.white;
        isActive = false;
        SetActive(false);
    }

    public void EndPrintText()
    {
        StopAllCoroutines();
        //isTextPrinting = false;
        HideBox();
    }

    public void SimpleTypePrint(string data)
    {
        bgImage.color = Color.black;
        bgImage.ChangeImageAlpha(1f);
        bgImage.sprite = simpleTypeSprite;

        messageText.SetText(data);
        messageText.color = currentColor;
        ShowBox();
        bgImage.rectTransform.sizeDelta = messageText.rectTransform.sizeDelta + offsetSize;
        bgImage.enabled = false;
        messageText.SetText("");
    }


    // 텍스트 길이가 길 시 자동 줄 바꿈
    private string SliceLineText(string text)
    {
        int cnt = 0; 
        for (int i = 0; i < text.Length; i++)
        {
            if(text[i] == '\n')
            {
                cnt = 0;
            }
            if(cnt > 40)
            {
                text = text.Insert(i, "\n");
                cnt = 0;
            }
            cnt++;
        }
        return text;
    }

    public void SetDelay(float value)
    {
        currentDelay = value;
    }
}