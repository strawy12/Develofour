using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EComputerLoginState
{
    Logout,
    Guest,
    Admin,
}

public class GameManager : MonoSingleton<GameManager>
{
    public Action OnStartCallback;

    [SerializeField]
    private ClickEffect clickEffect;

    private EGameState gameState;
    private EComputerLoginState computerLoginState;

    public EGameState GameState => gameState;
    public EComputerLoginState ComputerLoginState => computerLoginState;

    public bool isTutorial;

    public bool profilerTutorialClear;
    public bool isProfilerTownloadCompleted;

    public void ChangeGameState(EGameState state)
    {
        if (gameState == state) { return; }

        if (state == EGameState.Tutorial) isTutorial = true;

        gameState = state;

        if(gameState == EGameState.CutScene || gameState == EGameState.NotClick)
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

    public void ChangeComputerLoginState(EComputerLoginState state)
    {
        if (computerLoginState == state) return;

        computerLoginState = state;
    }

}
