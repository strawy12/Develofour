using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{


    public Action OnStartCallback;
    public Action<EGameState> OnChangeGameState;

    [SerializeField]
    private EGameState gameState;

    public EGameState GameState => gameState;

    [SerializeField]
    private GameObject cutSceneCanvas;
    public GameObject CutSceneCanvas => cutSceneCanvas;

    public bool IsTutorial => gameState == EGameState.Tutorial_NotChat;
    public bool isApplicationQuit { get; private set; }

#if UNITY_EDITOR
    public void Update()
    {
        Debug.Log(gameState);
    }
#endif

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


    private void OnApplicationQuit()
    {
        isApplicationQuit = true;
    }
}
