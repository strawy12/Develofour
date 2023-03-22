using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using static Sound;
using System.Collections.Generic;
using ExtenstionMethod;
using Unity.VisualScripting;

public class TextBox : MonoUI
{
    #region Binding 변수
    [SerializeField]
    private ContentSizeFitterText messageText;

    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private Image bgImage;
    #endregion

    #region 일반 변수

    #region SerializeField
    [SerializeField]
    private float printTextDelay = 0.05f;

    [SerializeField]
    private Vector2 offsetSize;

    [SerializeField]
    private Sprite simpleTypeSprite;

    [SerializeField]
    private Sprite boxTypeSprite;
    #endregion

    private bool isClick = false;
    public bool IsClick { get { return isClick; } }
    private TextDataSO currentTextData;
    private int currentTextIndex;
    private float currentDelay = 0f;
    private Dictionary<int, Action> triggerDictionary;

    public TextDataSO CurrentTextData { get => currentTextData; }
    #endregion

    #region Flag 변수
    private bool isTextPrinted = false;
    private bool isActive = false;
    private bool isEffected = false;
    private bool isFindSign = false;
    #endregion

    private void Start()
    {
        triggerDictionary = new Dictionary<int, Action>();
        EventManager.StartListening(ECoreEvent.OpenTextBox, Init);

        EventManager.StartListening(ETextboxEvent.Shake, SetShake);
        EventManager.StartListening(ETextboxEvent.Delay, SetDelay);
    }

    private void OnPressNextKey(object[] ps) => OnPressNextKey();
    private void OnPressNextKey()
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

    #region Init

    private void Init(object[] param)
    {
        if (param == null || !(param[0] is EMonologTextDataType))
        {
            return;
        }

        Init((EMonologTextDataType)param[0]);
        messageText.SetText("");

        ShowBox();
        PrintText();
    }

    public void Init(EMonologTextDataType textDataType)
    {
        if (GameManager.Inst.GameState != EGameState.CutScene)
        {
            GameManager.Inst.ChangeGameState(EGameState.UI);
        }

        currentTextData = ResourceManager.Inst.GetMonologTextDataSO(textDataType);
        currentTextIndex = 0;
    }

    #endregion

    #region PrintText
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

        SimpleTypePrint(textData);
        return textData.text.Length * printTextDelay;
    }

    public void SimpleTypePrint(TextData textData)
    {
        bgImage.color = Color.black;
        bgImage.ChangeImageAlpha(1f);
        bgImage.sprite = simpleTypeSprite;

        string settingText = RemoveCmd(textData.text);
        messageText.SetText(settingText);

        bgImage.rectTransform.sizeDelta = messageText.rectTransform.sizeDelta + offsetSize;
        messageText.SetText("");
        nameText.SetText("");
        messageText.color = textData.color;
        isTextPrinted = false;
        ShowBox();

        StartCoroutine(PrintMonologTextCoroutine(textData.text));
    }

    //{} 안에있는 내용을 없에줌
    private string RemoveCmd(string settingText)
    {
        List<Vector2> veclist = new List<Vector2>();
        string temp = string.Empty;

        int cnt = 0;
        for (int i = 0; i < settingText.Length; i++)
        {
            if (settingText[i] == '{')
            {
                veclist.Add(new Vector2(-1, -1));
                veclist[cnt] = new Vector2(i, -1);
            }
            if (settingText[i] == '}')
            {
                veclist[cnt] = new Vector2(veclist[cnt].x, i);
                cnt++;
            }
        }

        for (int i = veclist.Count; i > 0; i--)
        {
            if (veclist[i - 1].x != -1)
            {
                settingText = settingText.Remove((int)veclist[i - 1].x, ((int)veclist[i - 1].y - (int)veclist[i - 1].x) + 1);
            }
        }
        return settingText;
    }

    private IEnumerator PrintMonologTextCoroutine(string message)
    {
        bool isRich = false;
        bool isCmd = false;
        triggerDictionary.Clear();
        // 텍스트 박스 안에 넣을 텍스트
        // <color> 같은 것은 텍스트 박스 안에 넣어야함
        string textBoxInText = TextTrigger.RemoveCommandText(message, triggerDictionary, this.gameObject, true);
        // 텍스트가 너무 길 경우 자동으로 줄 바꿈 처리
        textBoxInText = SliceLineText(textBoxInText);

        messageText.SetText(textBoxInText);
        messageText.maxVisibleCharacters = 0;

        for (int i = 0; i < message.Length; i++)
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

            if (message[i] == '{')
            {
                isCmd = true;
            }

            if (message[i] == '}')
            {
                isCmd = false;
                continue;
            }

            if (!isCmd)
            {
                messageText.maxVisibleCharacters++;

                if (!isRich)
                {
                    Sound.OnPlaySound?.Invoke(EAudioType.MonologueTyping);
                }
            }

            if (triggerDictionary.ContainsKey(i))
            {
                triggerDictionary[i]?.Invoke();
            }

            // Rich 일때는 한번에 나오게 하기 위해서 딜레이 X
            if (!isRich && !isCmd)
            {
                yield return new WaitForSeconds(printTextDelay);
            }

            if (isCmd)
            {
                yield return new WaitForSeconds(currentDelay);
                currentDelay = 0f;
            }
        }

        if (isTextPrinted == false)
        {
            CompletePrint(textBoxInText);
        }

        isTextPrinted = false;

        EventManager.StartListening(EInputType.InputMouseDown, ClickEvent);

        yield return new WaitUntil(() => isClick);
        EventManager.StopListening(EInputType.InputMouseDown, ClickEvent);
        isClick = false;

        HideBox();
    }

    private void ClickEvent(object[] ps)
    {

        isClick = true;
    }

    #endregion

    #region TextBox 기본 함수
    public bool CheckDataEnd()
    {
        if (currentTextData == null)
        {
            GameManager.Inst.ChangeGameState(EGameState.Game);
            return true;
        }
        return currentTextIndex >= currentTextData.Count;
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
        nameText.SetText("");
        isActive = false;
        SetActive(false);

        InputManager.Inst.RemoveKeyInput(KeyCode.Space, onKeyUp: OnPressNextKey);
        EventManager.StopListening(EInputType.InputMouseUp, OnPressNextKey);
    }

    private void CompletePrint(string msg)
    {
        msg = ConversionPureText(msg);
        messageText.SetText(msg);
    }

    public void EndPrintText()
    {
        StopAllCoroutines();
        HideBox();
        isTextPrinted = false;
    }

    private void SkipPrintText()
    {
        if (!isTextPrinted) { return; }
        isTextPrinted = false;
    }


    #endregion

    #region Edit Text

    // 커맨드, Rich Text가 하나도 없는 순수 텍스트
    private string ConversionPureText(string text)
    {
        text = text.Replace("\r", "");

        if (text.Contains('<'))
        {
            text = RemoveRichText(text);
        }

        if (text.Contains('{'))
        {
            text = TextTrigger.RemoveCommandText(text, triggerDictionary, this.gameObject);
        }

        return text;
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

    // text에서 richText 뽑아냄 
    // ex) <color=#333333>
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



    // text에서 richText 없앰
    private string RemoveRichText(string message)
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

    // text에서 cmdText 없앰
    // 만약 registerCmd를 true할 시 커맨드 등록도 시킴
    //private string RemoveCommandText(string message, bool registerCmd = false)
    //{
    //    string removeText = message;
    //    int signTextLength = 0;


    //    for (int i = 0; i < removeText.Length; i++)
    //    {
    //        if (i < 0)
    //        {
    //            i = 0;
    //        }
    //        if (removeText[i] == '{')
    //        {
    //            isFindSign = true;

    //            string signText = TextTrigger.EncordingCommandText(removeText.Substring(i)); // {} 문자열
    //            removeText = removeText.Remove(i, signText.Length + 2); // {} 이 문자열을 제외시킨 문자열

    //                if (registerCmd)
    //                {

    //                    if (triggerDictionary.ContainsKey(i + signTextLength))
    //                        triggerDictionary[i + signTextLength] += () => TextTrigger.CommandTrigger(signText, this.gameObject);

    //                    else
    //                    {
    //                        triggerDictionary.Add(i + signTextLength, () => TextTrigger.CommandTrigger(signText, this.gameObject));
    //                    }
    //                }

    //            signTextLength += signText.Length;
    //            i -= signText.Length;
    //        }
    //    }

    //    return removeText;
    //}

    #endregion

    #region CommandTrigger

    public void SetDelay(object[] ps)
    {
        if (ps[0] is float)
        {
            currentDelay = (float)ps[0];
        }
    }

    public void SetShake(object[] ps)
    {
        //float delay, float strength, int vibrato, GameObject obj;
        if (ps[3] as GameObject == this.gameObject)
        {
            float delay = (float)ps[0];
            float strength = (float)ps[1];
            int vibrato = (int)ps[2];

            StartCoroutine(textShakingCoroutine(delay, strength, vibrato));
        }
    }

    private IEnumerator textShakingCoroutine(float delay, float strength, int vibrato)
    {
        isEffected = true;
        messageText.rectTransform.DOShakeAnchorPos(delay, strength, vibrato, 0, true);
        yield return new WaitForSeconds(delay);
        isEffected = false;
    }
    #endregion
}
