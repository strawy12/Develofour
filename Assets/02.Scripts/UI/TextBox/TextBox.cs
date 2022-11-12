using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TextBox : MonoUI
{
    private int currentTextIndex;

    [SerializeField]
    private TMP_Text boxShowText;

    [SerializeField]
    private TextDataSO currentTextData;

    private Queue<char> soundEffectQueue = new Queue<char>();

    private bool isSoundEffect;

    [SerializeField]
    private float printTextDuration = 0.05f;

    private void Start()
    {
        EventManager.StartListening(EEvent.OpenTextBox, Init);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            ShowBox();
        }
    }

    private void Init(object param)
    {
        if(param == null || !(param is ETextDataType))
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
        if(currentTextIndex >= currentTextData.Count)
        {
            return;
        }

        boxShowText.SetText(currentTextData[currentTextIndex]);
        currentTextIndex++;
    }

    public IEnumerator PrintTextCoroutine(string message)
    {
        boxShowText.text = "";
        string text= "";
        foreach(var c in message)
        {
            text = string.Format("{0}{1}", text, c);
            boxShowText.text = text;
            yield return new WaitForSeconds(printTextDuration);
        }
        boxShowText.text = message;
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
}
