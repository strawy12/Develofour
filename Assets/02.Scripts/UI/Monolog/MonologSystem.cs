using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MonologSystem : TextSystem
{
    /// <summary>
    /// int textDataType, float beforeDelay, bool isSave
    /// </summary>
    public static Action<int, float, bool> OnStartMonolog { get; private set; }
    public static Action OnStopMonolog { get; private set; }

    private static Queue<Action> onEndMonologEventLQueue;

    public static bool isEndMonolog { get; private set; }

    private static Action onEndMonologEvent;
    public static Action OnEndMonologEvent
    {
        set
        {
            if (isEndMonolog)
            {
                if (onEndMonologEventLQueue == null)
                    onEndMonologEventLQueue = new Queue<Action>();

                onEndMonologEventLQueue.Enqueue(value);
            }

            else
            {
                onEndMonologEvent += value;
            }
        }
    }

    public static void RemoveEndMonologEvent(Action action)
    {
        onEndMonologEvent -= action;
    }

    [SerializeField]
    private TextBox textBox;

    private TextDataSO currentTextData;
    private int currentTextDataIdx = 0;

    private EGameState beforeGameState;

    private void Awake()
    {
        onEndMonologEventLQueue = new Queue<Action>();

        OnStartMonolog += StartMonolog;
        OnStopMonolog += StopMonolog;

        EventManager.StartListening(EMonologEvent.MonologException, ProfileFileException);
    }

    public void StartMonolog(int textDataType, float beforeDelay, bool isSave)
    {
        StartCoroutine(StartMonologCor(textDataType, beforeDelay, isSave));
    }

    public IEnumerator StartMonologCor(int textDataType, float beforeDelay, bool isSave)
    {
        yield return new WaitUntil(() => !isEndMonolog);

        beforeGameState = GameManager.Inst.GameState;
        GameManager.Inst.ChangeGameState(EGameState.CutScene);

        yield return new WaitForSeconds(beforeDelay);

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

        EventManager.TriggerEvent(EMonologEvent.MonologEnd);

        textBox.DictionaryClear();

        isEndMonolog = true;
         onEndMonologEvent?.Invoke();
        onEndMonologEvent = null;
        isEndMonolog = false;

        AddEndMonologEvent();
    }

    private void AddEndMonologEvent()
    {
        while (onEndMonologEventLQueue.Count > 0)
        {
            onEndMonologEvent += onEndMonologEventLQueue.Dequeue();
        }
    }

    private void StopMonolog()
    {
        textBox.HideBox();
        if (currentTextData == null)
        {
            return;
        }
        currentTextDataIdx = currentTextData.Count;
        InputManager.Inst.RemoveAnyKeyInput(onKeyDown: null);
        GameManager.Inst.ChangeGameState(beforeGameState);
        textBox.DictionaryClear();
        onEndMonologEvent?.Invoke();
        onEndMonologEvent = null;
        AddEndMonologEvent();
    }

    private void PrintText()
    {
        if(currentTextData == null)
        {
            EndMonolog();
            return;
        }
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

        TextData textData = currentTextData[currentTextDataIdx++];
        string text = RemoveCommandText(textData.text, true);

        // TextBox 한테 일시키기
        // {}
        textBox.Init(text, triggerDictionary, textData.textColor);
    }

    public override void SetDelay(float value)
    {
        textBox.SetDelay(value);
    }
}
