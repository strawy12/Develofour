using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CutScene : MonoBehaviour
{
    protected bool isPlaying;

    void Start()
    {
        EventManager.StartListening(EEvent.ShowCutScene, CheckStart);
    }

    private void CheckStart(object param)
    {
        if (isPlaying) return;
        if (param == null || !(param is Type))
        {
            return;
        }

        if (GetType() != (Type)param)
        {
            return;
        }

        StartCutScene();
        ShowCutScene();
    }

    protected virtual void StartCutScene()
    {
        EventManager.StartListening(EEvent.SkipCutScene, SkipCutScene);

        gameObject.SetActive(true);
        isPlaying = true;
        GameManager.Inst.ChangeGameState(EGameState.CutScene);
    }

    protected virtual void EndCutScene()
    {
        EventManager.StopListening(EEvent.SkipCutScene, SkipCutScene);
        isPlaying = false;
        GameManager.Inst.ChangeGameState(EGameState.Game);
        gameObject.SetActive(false);
    }

    private void SkipCutScene(object o)
    {
        StopAllCoroutines();
        EndCutScene();
    }


    protected abstract void ShowCutScene();
}
