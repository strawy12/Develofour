using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MonologSystem : TextSystem
{
    public static Action<EMonologTextDataType, float, bool> OnStartMonolog { get; private set; }
    public static Action OnStopMonolog { get; private set; }

    public static Action OnEndMonologEvent;

    [SerializeField]
    private TextBox textBox;

    private TextDataSO currentTextData;
    private int currentTextDataIdx = 0;

    private EGameState beforeGameState;

    private void Awake()
    {
        OnStartMonolog += StartMonolog;
        OnStopMonolog += StopMonolog;

        EventManager.StartListening(EMonologEvent.MonologException, ProfileFileException);
    }
    
    public void StartMonolog(EMonologTextDataType textDataType, float beforeDelay, bool isSave)
    {
        StartCoroutine(StartMonologCor(textDataType, beforeDelay, isSave));
    }

    public IEnumerator StartMonologCor(EMonologTextDataType textDataType, float beforeDelay, bool isSave)
    {

        if (DataManager.Inst.IsMonologShow(textDataType))
        {
            OnEndMonologEvent = null;
            yield break;
        }

        yield return new WaitForSeconds(beforeDelay);

        beforeGameState = GameManager.Inst.GameState;
        GameManager.Inst.ChangeGameState(EGameState.CutScene);

        currentTextDataIdx = 0;

        currentTextData = ResourceManager.Inst.GetMonologTextData(textDataType);

        PrintText();
        InputManager.Inst.AddAnyKeyInput(onKeyDown: PrintText);

        if (isSave)
        {
            DataManager.Inst.SetMonologShow(textDataType, true);
        }
    }

    private void EndMonolog()
    {
        textBox.HideBox();
        InputManager.Inst.RemoveAnyKeyInput(onKeyDown: PrintText);
        GameManager.Inst.ChangeGameState(beforeGameState);

        textBox.DictionaryClear();

        OnEndMonologEvent?.Invoke();
        OnEndMonologEvent = null;
    }

    private void StopMonolog()
    {
        textBox.HideBox();
        currentTextDataIdx = currentTextData.Count;
        InputManager.Inst.RemoveAnyKeyInput(onKeyDown: null);
        GameManager.Inst.ChangeGameState(beforeGameState);
        textBox.DictionaryClear();
        OnEndMonologEvent?.Invoke();
        OnEndMonologEvent = null;
    }

    private void PrintText()
    {
        if (textBox.isTextPrinting)
        {
            return;
        }
        //triggerDictionary = new Dictionary<int, Action>();
        if (currentTextData.Count == currentTextDataIdx)
        {
            EndMonolog();
            return;
        }
        TextData data = currentTextData[currentTextDataIdx++];
        string text = data.text;
        text = RemoveCommandText(text, true);

        // TextBox 한테 일시키기
        // {}

        textBox.Init(data, text, triggerDictionary);
    }   

    public override void SetDelay(float value)
    {
        textBox.SetDelay(value);
    }
}
