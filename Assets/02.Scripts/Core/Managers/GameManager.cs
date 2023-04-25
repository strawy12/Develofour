using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Action OnStartCallback;
    public Action<EGameState> OnChangeGameState;

    private EGameState gameState;

    public EGameState GameState => gameState;


    public bool IsTutorial => gameState == EGameState.Tutorial;

    public void Init()
    {
        DataManager.Inst.Init();
        FileManager.Inst.Init();
        OnStartCallback?.Invoke();
    }

    public void ChangeGameState(EGameState state)
    {
        if (gameState == state) { return; }

        gameState = state;

        OnChangeGameState?.Invoke(gameState);

        if (gameState == EGameState.CutScene || gameState == EGameState.NotClick)
        {
            EventManager.TriggerEvent(ECoreEvent.CoverPanelSetting, new object[1] { true });
        }
        else
        {
            EventManager.TriggerEvent(ECoreEvent.CoverPanelSetting, new object[1] { false });
        }

    }

    public void ClickStop(float time)
    {
        StartCoroutine(ChangeStopClickState(time, gameState));
        ChangeGameState(EGameState.NotClick);
    }

    private IEnumerator ChangeStopClickState(float time, EGameState state)
    {
        yield return new WaitForSeconds(time);
        ChangeGameState(state);
    }

}
