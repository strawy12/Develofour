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
        isPlaying = true;
        GameManager.Inst.ChangeGameState(EGameState.CutScene);
    }

    protected virtual void EndCutScene()
    {
        isPlaying = false;
        GameManager.Inst.ChangeGameState(EGameState.UI);
    }

    protected abstract void ShowCutScene();
}
