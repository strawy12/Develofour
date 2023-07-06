using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologSystem : TextSystem
{
    /// <summary>
    /// int textDataType, bool isSave
    /// </summary>
    public static Action<string, bool> OnStartMonolog { get; private set; }
    public static Action OnStopMonolog { get; private set; }

    private static Dictionary<string, Action> onEndMonologDictionary;

    private bool isPlayMonolog = false;

    [SerializeField]
    private TextBox textBox;

    private MonologTextDataSO currentTextData;
    private int currentTextDataIdx = 0;

    private EGameState beforeGameState;

    private void Awake()
    {
        onEndMonologDictionary = new Dictionary<string, Action>();

        OnStartMonolog += StartMonolog;
        OnStopMonolog += EndMonolog;

        //EventManager.StartListening(EMonologEvent.MonologException, ProfilerFileException);
    }

    public static void AddOnEndMonologEvent(string monologID, Action action)
    {
        if (onEndMonologDictionary.ContainsKey(monologID))
        {
            onEndMonologDictionary[monologID] += action;
        }
        else
        {
            onEndMonologDictionary.Add(monologID, action);
        }
    }

    public void StartMonolog(string textDataType, bool isSave)
    {
        if (isPlayMonolog) return;

        if (isSave)
        {
            if (DataManager.Inst.IsMonologShow(textDataType))
            {
                Debug.LogError("이미 저장된 독백입니다");
                return;
            }
        }

        isPlayMonolog = true;
        beforeGameState = GameManager.Inst.GameState;
        GameManager.Inst.ChangeGameState(EGameState.CutScene);

        currentTextDataIdx = 0;
        currentTextData = ResourceManager.Inst.GetMonologTextData(textDataType);

        if (currentTextData == null)
        {
            Debug.LogError("해당 독백은 존재하지 않습니다: " + textDataType);
        }

        PrintText();
        InputManager.Inst.AddAnyKeyInput(onKeyDown: PrintText);
        DataManager.Inst.SetMonologShow(textDataType, true);
    }

    private void EndMonolog()
    {
        if (currentTextData == null)
            return;

        textBox.HideBox();

        InputManager.Inst.RemoveAnyKeyInput(onKeyDown: PrintText);
        GameManager.Inst.ChangeGameState(beforeGameState);

        EventManager.TriggerEvent(EMonologEvent.MonologEnd);

        textBox.DictionaryClear();

        if (currentTextData != null)
        {
            Action onEndEvent = onEndMonologDictionary[currentTextData.ID];
            onEndEvent?.Invoke();
            onEndMonologDictionary.Remove(currentTextData.ID);
        }
    }

    private void PrintText()
    {
        if (currentTextData == null)
        {
            EndMonolog();
            return;
        }
        if (textBox.isTextPrinting)
        {
            textBox.ImmediatelyComplete();
            return;
        }
        //triggerDictionary = new Dictionary<int, Action>();
        if (currentTextData.Count <= currentTextDataIdx)
        {
            EndMonolog();
            return;
        }

        TextData textData = currentTextData[currentTextDataIdx];
        string text = RemoveCommandText(textData.text, true);

        // TextBox 한테 일시키기
        // {}
        textBox.OnEnd += () => currentTextDataIdx++;
        textBox.Init(text, triggerDictionary, textData.textColor);
    }

    public override void SetDelay(float value)
    {
        textBox.SetDelay(value);
    }
}
