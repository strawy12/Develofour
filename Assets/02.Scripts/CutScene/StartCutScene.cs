using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.Rendering;

public class StartCutScene : MonoBehaviour
{
    public static Action OnPlayCutScene { get; private set; }

    [Header("디버그용")]
    public bool isSkip;

    public TMP_Text titleText;

    public Image interrogationRoomSprite;

    [SerializeField]
    private GameObject blackImagePanel;

    [SerializeField]
    private Image backgroundImagePanel;

    [SerializeField]
    private LoadingIcon loadingIcon;

    [SerializeField]
    private GameObject loadingText;


    [Header("디버그용")]
    public bool isScreamSound;


    public void Init()
    {
        this.gameObject.SetActive(true);
        if (DataManager.Inst.SaveData.isWatchStartCutScene)
        {
            EndRequestCutScene();
            GameManager.Inst.ChangeGameState(EGameState.Game);
            Destroy(this.gameObject);
        }
        else
        {
            OnPlayCutScene += CutSceneStart;
        }
    }

    private void CutSceneStart()
    {
        StartCoroutine(PlayCutSceneCoroutine());
    }

    private IEnumerator PlayCutSceneCoroutine()
    {
        GameManager.Inst.OnChangeGameState(EGameState.CutScene);
        EventManager.TriggerEvent(ECoreEvent.OpenVolume, new object[] { true });
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.StartCutSceneScream);
        isScreamSound = true;
        yield return new WaitForSeconds(15f);
        isScreamSound = false;
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.StartCutScenePoint);
        titleText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        titleText.DOColor(new Color(255, 255, 255, 0), 2.5f);
        yield return new WaitForSeconds(3f);

        yield return new WaitForSeconds(1f);
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.StartCutSceneLightPull);
        EventManager.TriggerEvent(ECoreEvent.OpenVolume, new object[] { true });
        interrogationRoomSprite.DOFade(1, 2f);
        yield return new WaitForSeconds(1.5f);
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.InterrogationRoom);

        MonologSystem.OnEndMonologEvent += FadeInterrogationRoomSprite;
        MonologSystem.OnStartMonolog(EMonologTextDataType.StartCutSceneMonolog1, 0, true);
    }

    private void FadeInterrogationRoomSprite()
    {
        StartCoroutine(FadeInterrogationRoomSpriteCor());
    }

    private IEnumerator FadeInterrogationRoomSpriteCor()
    {
        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.InterrogationRoom);

        interrogationRoomSprite.DOFade(0, 1.5f);
        yield return new WaitForSeconds(2f);

        Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneAlarm);
        yield return new WaitForSeconds(3.3f);

        Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneReceive);
        StartRequest();
    }

    private void StartRequest()
    {
        backgroundImagePanel.DOFade(1, 1.5f);
        MonologSystem.OnEndMonologEvent += StartLoading;
        MonologSystem.OnStartMonolog?.Invoke(EMonologTextDataType.StartCutSceneMonolog2, 1.5f, true);
    }

    public void StartLoading()
    {
        EventManager.TriggerEvent(ECoreEvent.OpenVolume, new object[] { false });
        StartCoroutine(StartLoadingCor());
    }

    private IEnumerator StartLoadingCor()
    {
        backgroundImagePanel.DOFade(0, 1.5f);
        yield return new WaitForSeconds(2f);
        EventManager.TriggerEvent(ECoreEvent.OpenVolume, new object[] { false });
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
