using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    protected bool isPlaying;

    private EGameState saveState;

    public virtual void ShowCutScene()
    {
        if (isPlaying) return;
        GameManager.Inst.cutSceneCoverPanel.gameObject.SetActive(true);
        saveState = GameManager.Inst.GameState;
        Sound.OnStopBGM?.Invoke(true);
        isPlaying = true;
        InputManager.Inst.AddKeyInput(KeyCode.Escape, onKeyDown: StopCutScene);
    }
    public virtual void StopCutScene()
    {
        if (!isPlaying) return;
        GameManager.Inst.cutSceneCoverPanel.gameObject.SetActive(false);
        GameManager.Inst.ChangeGameState(saveState);
        isPlaying = false;
        InputManager.Inst.RemoveKeyInput(KeyCode.Escape, onKeyDown: StopCutScene);
        StopAllCoroutines();
        Sound.OnPlayLastBGM?.Invoke();
        MonologSystem.OnStopMonolog?.Invoke();
        Destroy(this.gameObject);
    }
    protected void StartMonolog(int monoID, float delay = 0f)
    {
        MonologSystem.OnStartMonolog?.Invoke(monoID, delay, true);
    }

    public virtual void OnDestroy()
    {
        GameManager.Inst.cutSceneCoverPanel.gameObject.SetActive(false);
    }
}
