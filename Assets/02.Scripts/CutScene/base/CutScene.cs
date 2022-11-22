using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CutScene : MonoBehaviour
{
    protected bool isPlaying;

    void Start()
    {
        EventManager.StartListening(ECutSceneEvent.ShowCutScene, CheckStart);
    }

    private void CheckStart(object[] param)
    {
        if (isPlaying) return;
        if (param == null || !(param[0] is Type))
        {
            return;
        }

        if (GetType() != (Type)param[0])
        {
            return;
        }

        StartCutScene();
        ShowCutScene();
    }

    protected virtual void StartCutScene()
    {
        EventManager.StartListening(ECutSceneEvent.SkipCutScene, SkipCutScene);

        gameObject.SetActive(true);
        isPlaying = true;
        GameManager.Inst.ChangeGameState(EGameState.CutScene);
    }

    protected virtual void EndCutScene()
    {
        EventManager.StopListening(ECutSceneEvent.SkipCutScene, SkipCutScene);
        isPlaying = false;
        GameManager.Inst.ChangeGameState(EGameState.Game);
        gameObject.SetActive(false);
    }

    private void SkipCutScene(object[] o)
    {
        StopAllCoroutines();
        EndCutScene();
    }


    protected abstract void ShowCutScene();
}
