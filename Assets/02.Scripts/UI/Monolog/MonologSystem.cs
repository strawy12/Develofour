using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologSystem : TextSystem
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
    }

    public void StartMonolog(EMonologTextDataType textDataType, float beforDelay, bool isSave)
    {
        if (DataManager.Inst.IsMonologShow(textDataType))
        {
            OnEndMonologEvent = null;
            return;
        }

        beforeGameState = GameManager.Inst.ChangeGameState(EGameState.CutScene);

        //textBox.Init();

        currentTextDataIdx = 0;

        InputManager.Inst.AddAnyKeyInput(onKeyDown: PrintText);

        currentTextData = ResourceManager.Inst.GetMonologTextData(textDataType);

        if(isSave)
        {
            DataManager.Inst.SetMonologShow(textDataType, true);
        }
    }

    private void EndMonolog()
    {
        GameManager.Inst.ChangeGameState(beforeGameState);
    }

    private void StopMonolog()
    {
        OnEndMonologEvent?.Invoke();
        StopAllCoroutines();
        //textBox.Release();
        GameManager.Inst.ChangeGameState(EGameState.Game);
        OnEndMonologEvent = null;
    }

    private void PrintText()
    {
        TextData data = currentTextData[currentTextDataIdx++];
        data.text = RemoveCommandText(data.text, true);

        // TextBox 한테 일시키기
        // {}

        textBox.Init(data, triggerDictionary);
    }

    public override void SetDelay(float value)
    {
        textBox.SetDelay(value);
    }
}
