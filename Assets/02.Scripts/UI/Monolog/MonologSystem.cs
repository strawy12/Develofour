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

    private static bool isPlayMonolog = false;
    public static bool IsPlayMonolog { get; }

    [SerializeField]
    private TextBox textBox;

    private static MonologTextDataSO currentTextData;

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

    public void StartMonolog(string monologID, bool isSave)
    {
        if (isPlayMonolog) return;

        if (isSave)
        {
            if (DataManager.Inst.IsMonologShow(monologID))
            {
                Debug.LogError("이미 저장된 독백입니다");
                return;
            }
        }

        isPlayMonolog = true;
        beforeGameState = GameManager.Inst.GameState;
        GameManager.Inst.ChangeGameState(EGameState.CutScene);

        currentTextDataIdx = 0;

        currentTextData = ResourceManager.Inst.GetResource<MonologTextDataSO>(monologID);

        if (currentTextData == null)
        {
            Debug.LogError("해당 독백은 존재하지 않습니다: " + monologID);
        }

        PrintText();
        InputManager.Inst.AddAnyKeyInput(onKeyDown: PrintText);
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

        isPlayMonolog = false;

        if (currentTextData != null)
        {
            if(!onEndMonologDictionary.ContainsKey(currentTextData.ID))
            {
                return;
            }
            Action onEndEvent = onEndMonologDictionary[currentTextData.ID];
            onEndEvent?.Invoke();
            Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.None);
            onEndMonologDictionary.Remove(currentTextData.ID);
            DataManager.Inst.SetMonologShow(currentTextData.ID);
            currentTextData = null;
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
