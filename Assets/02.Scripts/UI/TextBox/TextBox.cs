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
    public enum ETextBoxType
    {
        Simple,
        Box,
    }
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
    private ETextBoxType currentType;
    private Dictionary<int, Action> triggerDictionary;
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

    #region Init

    private void Init(object[] param)
    {
        if (param == null || !(param[0] is ETextDataType))
        {
            return;
        }

        ETextBoxType boxType = ETextBoxType.Box;

        if (param.Length >= 2 && param[1] is ETextBoxType)
        {
            boxType = (ETextBoxType)param[1];
        }

        Init((ETextDataType)param[0], boxType);
        ShowBox();
        PrintText();
    }

    public void Init(ETextDataType textDataType, ETextBoxType textBoxType = ETextBoxType.Box)
    {
        if (GameManager.Inst.GameState != EGameState.CutScene)
        {
            GameManager.Inst.ChangeGameState(EGameState.UI);
        }

        SetTextBoxType(textBoxType);

        currentTextData = GetTextData(textDataType);
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

        if (currentType == ETextBoxType.Box)
        {
            BoxTypePrint(textData);
            return textData.text.Length * printTextDelay;
        }

        else
        {
            SimpleTypePrint(textData);
            return textData.text.Length * printTextDelay;
        }
    }

    public void SimpleTypePrint(TextData textData)
    {
        bgImage.color = Color.black;
        bgImage.ChangeImageAlpha(0.7f);
        bgImage.sprite = simpleTypeSprite;

        messageText.SetText(textData.text);
        bgImage.rectTransform.sizeDelta = messageText.rectTransform.sizeDelta + offsetSize;
        messageText.SetText("");
        nameText.SetText("");
        messageText.color = textData.color;
        isTextPrinted = false;
        ShowBox();
        StartCoroutine(PrintMonologTextCoroutine(textData.text));
    }


    private void BoxTypePrint(TextData textData)
    {
        bgImage.color = Color.white;
        bgImage.sprite = boxTypeSprite;

        RectTransform parent = bgImage.transform.parent as RectTransform;
        bgImage.rectTransform.sizeDelta = new Vector2(parent.rect.width, parent.rect.height);

        triggerDictionary.Clear();
        messageText.SetText("");
        nameText.SetText(textData.name);
        nameText.color = textData.color;
        StartCoroutine(PrintTextCoroutine(textData.text));
    }
    
    private IEnumerator PrintTextCoroutine(string message)
    {
        bool isRich = false;

        // 모든 표시를 지운 순수 텍스트
        string removeSignText = ConversionPureText(message);

        // 텍스트 박스 안에 넣을 텍스트
        // <color> 같은 것은 텍스트 박스 안에 넣어야함
        string textBoxInText = RemoveCommandText(message, true);

        // 텍스트가 너무 길 경우 자동으로 줄 바꿈 처리
        textBoxInText = SliceLineText(textBoxInText);

        messageText.SetText(textBoxInText);

        for (int i = 0; i < textBoxInText.Length; i++)
        {
            if (textBoxInText[i] == '<')
            {
                isRich = true;
            }

            if (textBoxInText[i] == '>')
            {
                isRich = false;
                continue;
            }

            messageText.maxVisibleCharacters = i;

            // 미리 생성시킨 
            if (triggerDictionary.ContainsKey(i))
            {
                triggerDictionary[i]?.Invoke();
            }

            // Rich 일때는 한번에 나오게 하기 위해서 딜레이 X
            if (!isRich)
            {
                yield return new WaitForSeconds(printTextDelay);
            }
        }

        if (isTextPrinted == false)
        {
            CompletePrint(textBoxInText);
        }

        isTextPrinted = false;
    }
    private IEnumerator PrintMonologTextCoroutine(string message)
    {
        bool isRich = false;

        // 모든 표시를 지운 순수 텍스트
        string removeSignText = ConversionPureText(message);

        // 텍스트 박스 안에 넣을 텍스트
        // <color> 같은 것은 텍스트 박스 안에 넣어야함
        string textBoxInText = RemoveCommandText(message, true);

        // 텍스트가 너무 길 경우 자동으로 줄 바꿈 처리
        textBoxInText = SliceLineText(textBoxInText);

        messageText.SetText(textBoxInText);

        for (int i = 0; i < textBoxInText.Length; i++)
        {
            if (textBoxInText[i] == '<')
            {
                isRich = true;
            }

            if (textBoxInText[i] == '>')
            {
                isRich = false;
                continue;
            }

            messageText.maxVisibleCharacters = i;

            // 미리 생성시킨 
            if (triggerDictionary.ContainsKey(i))
            {
                triggerDictionary[i]?.Invoke();
            }

            // Rich 일때는 한번에 나오게 하기 위해서 딜레이 X
            if (!isRich)
            {
                yield return new WaitForSeconds(printTextDelay);
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
    public void SetTextBoxType(ETextBoxType type)
    {
        currentType = type;
    }
    public bool CheckDataEnd()
    {
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
            text = RemoveCommandText(text);
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

    // text에서 cmdText 뽑아냄
    // ex) {BS_WriterBGM}
    private string EncordingCommandText(string message)
    {
        string richText = "";

        if (message[0] == '{')
        {
            message = message.Substring(1);
        }

        for (int i = 0; i < message.Length; i++)
        {
            richText += message[i];

            if (message[i] == '}')
            {
                break;
            }
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
    private string RemoveCommandText(string message, bool registerCmd = false)
    {
        string removeText = message;

        for (int i = 0; i < removeText.Length; i++)
        {
            if (removeText[i] == '{')
            {
                isFindSign = true;

                string signText = EncordingCommandText(removeText.Substring(i)); // {} 문자열
                removeText = removeText.Remove(i, signText.Length); // {} 이 문자열을 제외시킨 문자열
                i -= signText.Length;

                if (registerCmd)
                {
                    triggerDictionary.Add(i, () => CommandTrigger(signText));
                }
            }
        }

        return removeText;
    }

    #endregion

    #region CommandTrigger

    private int CommandTrigger(string msg)
    {
        string cmdMsg = EncordingCommandText(msg);
        int cnt = 1;

        string[] cmdMsgSplit = cmdMsg.Split('_');
        string cmdType = cmdMsgSplit[0];
        string cmdValue = cmdMsgSplit[1];

        switch (cmdType)
        {
            case "ES":
                {
                    Sound.EEffect effectType = (EEffect)Enum.Parse(typeof(EEffect), cmdValue);
                    Sound.OnPlayEffectSound?.Invoke(effectType);
                    break;
                }
            case "BS":
                {
                    Sound.EBgm bgmType = (EBgm)Enum.Parse(typeof(EBgm), cmdValue);
                    Sound.OnPlayBGMSound?.Invoke(bgmType);
                    break;
                }
            case "SK":
                {
                    string[] cmdValueArray = cmdValue.Split(',');
                    float delay = float.Parse(cmdValueArray[0]);
                    float strength = float.Parse(cmdValueArray[1]);
                    int vibrato = int.Parse(cmdValueArray[2]);

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
    #endregion

    #region TextData 관련

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
    #endregion 

}
