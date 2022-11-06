using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CutScene : MonoBehaviour
{
    void Start()
    {
        EventManager.StartListening(EEvent.ShowCutScene, CheckStart);
    }

    private void CheckStart(object param)
    {
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
        GameManager.Inst.ChangeGameState(EGameState.CutScene);
    }

    protected virtual void EndCutScene()
    {
        GameManager.Inst.ChangeGameState(EGameState.UI);
    }

    protected abstract void ShowCutScene();
}
