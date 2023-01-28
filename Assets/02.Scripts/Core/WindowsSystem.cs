using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsSystem : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup windowsCanvas;

    [SerializeField]
    private CanvasGroup windowsLoginCanvas;

    private void Start()
    {
        //EventManager.StartListening(EQuestEvent.WriterWindowsLoginSuccess, OpenWindowsCanvas);
    }

    private void OpenWindowsCanvas(object[] empty)  
    {
        windowsCanvas.alpha = 0;

        windowsCanvas.DOFade(1f, 0.5f);
        windowsLoginCanvas.DOFade(0f, 0.75f).OnComplete(() => windowsLoginCanvas.gameObject.SetActive(false));

        //EventManager.StopListening(EQuestEvent.WriterWindowsLoginSuccess, OpenWindowsCanvas);
    }
}
