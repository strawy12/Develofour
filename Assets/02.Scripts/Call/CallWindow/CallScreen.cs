using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallScreen : MonoBehaviour
{
    protected bool isPlaying;

    private EGameState saveState;
    public GameObject callCoverPanel;

    [SerializeField]
    private List<Image> userImageList;

    public virtual void StartCall()
    {
        if (isPlaying) return;
        callCoverPanel.SetActive(true);
        saveState = GameManager.Inst.GameState;
        isPlaying = true;
    }

    protected void StartMonolog(string monologID)
    {
        MonologSystem.OnStartMonolog?.Invoke(monologID, true);
    }
    public virtual void StopCall()
    {
        if (!isPlaying) return;

        GameManager.Inst.ChangeGameState(saveState);
        isPlaying = false;
        StopAllCoroutines();
        MonologSystem.OnStopMonolog?.Invoke();
        callCoverPanel.SetActive(false);
        Destroy(this.gameObject);
    }
    public virtual void OnDestroy()
    {

    }
}
