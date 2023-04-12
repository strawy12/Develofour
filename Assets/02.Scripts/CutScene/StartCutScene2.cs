using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class StartCutScene2 : MonoBehaviour
{

    [SerializeField]
    private GameObject blackImagePanel;

    [SerializeField]
    private Image backgroundImagePanel;

    [SerializeField]
    private LoadingIcon loadingIcon;

    [SerializeField]
    private GameObject loadingText;


    private void Start()
    {
        if (DataManager.Inst.SaveData.isWatchStartCutScene)
        {
            EndRequestCutScene();
            GameManager.Inst.ChangeGameState(EGameState.Game);
            Destroy(this.gameObject);
        }
        else
        {
            EventManager.StartListening(ECoreEvent.EndDataLoading, StartRequest);
        }
    }
    private void StartRequest(object[] ps)
    {
        backgroundImagePanel.DOFade(1, 1.5f);
        MonologSystem.OnEndMonologEvent += StartLoading;
        MonologSystem.OnStartMonolog?.Invoke(EMonologTextDataType.StartCutSceneMonolog2, 1.5f, true);
    }

    private void StartLoading()
    {
        backgroundImagePanel.gameObject.SetActive(false);
        blackImagePanel.gameObject.SetActive(true);
        loadingIcon.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(true);
        loadingIcon.StartLoading(1.5f, EndRequestCutScene);
    }

    
    private void EndRequestCutScene()
    {
        DataManager.Inst.SaveData.isWatchStartCutScene = true;
        GameManager.Inst.ChangeGameState(EGameState.Game);
        EventManager.TriggerEvent(ECutSceneEvent.EndStartCutScene);
        Sound.OnPlaySound(Sound.EAudioType.StartMainBGM);

        SetActiveThisObject();
    }
    


    private void SetActiveThisObject()
    {
        Destroy(gameObject);
    }
}
