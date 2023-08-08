using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    protected bool isPlaying;
    protected bool isCanStop = true;

    private EGameState saveState;
    public GameObject cutSceneCoverPanel;

    public virtual void ShowCutScene()
    {
        if (isPlaying) return;
        cutSceneCoverPanel.SetActive(true);
        saveState = GameManager.Inst.GameState;
        Sound.OnStopBGM?.Invoke(true);
        isPlaying = true;
        InputManager.Inst.AddKeyInput(KeyCode.Escape, onKeyDown: ClickESC);
    }

    private void ClickESC()
    {
        if (isCanStop == false) return;

        StopCutScene();
    }

    public virtual void StopCutScene()
    {
        if (!isPlaying) return;
        cutSceneCoverPanel.SetActive(false);
        GameManager.Inst.ChangeGameState(saveState);
        isPlaying = false;
        InputManager.Inst.RemoveKeyInput(KeyCode.Escape, onKeyDown: ClickESC);
        StopAllCoroutines();
        Sound.OnPlayLastBGM?.Invoke();
        MonologSystem.OnStopMonolog?.Invoke();
        Destroy(this.gameObject);
    }
    protected void StartMonolog(string monologID)
    {
        MonologSystem.OnStartMonolog?.Invoke(monologID, true);
    }

    public virtual void OnDestroy()
    {
        cutSceneCoverPanel.SetActive(false);
    }
}
