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

    [SerializeField]
    private Image titleLogo;

    [SerializeField]
    private Image interrogationRoomSprite;

    [SerializeField]
    private GameObject blackImagePanel;

    [SerializeField]
    private Image backgroundImagePanel;

    [SerializeField]
    private LoadingIcon loadingIcon;

    [SerializeField]
    private GameObject loadingText;

    [SerializeField]
    private GameObject cutSceneCoverPanel;

    [Header("디버그용")]
    public bool isScreamSound;
    public bool isSkip;

    private void Awake()
    {
        OnPlayCutScene += CutSceneStart;
    }

    private void CutSceneStart()
    {
        OnPlayCutScene -= CutSceneStart;
        cutSceneCoverPanel.SetActive(true);
        if (DataManager.Inst.SaveData.isWatchStartCutScene)
        {
            EndRequestCutScene();
            GameManager.Inst.ChangeGameState(EGameState.Game);
            Destroy(this.gameObject);
        }
        else
        {
            this.gameObject.SetActive(true);
            StartCoroutine(PlayCutSceneCoroutine());
        }
    }

    private IEnumerator PlayCutSceneCoroutine()
    {
        GameManager.Inst.OnChangeGameState?.Invoke(EGameState.CutScene);
        EventManager.TriggerEvent(ECoreEvent.OpenVolume, new object[] { true });
        float? delay = Sound.OnPlaySound?.Invoke(Sound.EAudioType.StartCutSceneScream);
        isScreamSound = true;
        yield return new WaitForSeconds(delay == null ? 5f : (float)delay);
        isScreamSound = false;
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.StartCutScenePoint);
        titleLogo.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        titleLogo.DOColor(new Color(255, 255, 255, 0), 2.5f);
        yield return new WaitForSeconds(3f);

        yield return new WaitForSeconds(1f);
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.StartCutSceneLightPull);
        EventManager.TriggerEvent(ECoreEvent.OpenVolume, new object[] { true });
        interrogationRoomSprite.DOFade(1, 2f);
        yield return new WaitForSeconds(1.5f);
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.InterrogationRoom);

        string monologID = Constant.MonologKey.STARTCUTSCENE_1;
        MonologSystem.AddOnEndMonologEvent(monologID, FadeInterrogationRoomSprite);
        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
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
        backgroundImagePanel.DOFade(1, 1.5f).OnComplete(() =>
        {
            string monologID = Constant.MonologKey.STARTCUTSCENE_2;
            MonologSystem.AddOnEndMonologEvent(monologID, StartLoading);
            MonologSystem.OnStartMonolog?.Invoke(monologID, false);
        });
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
        cutSceneCoverPanel.SetActive(false);
        GameManager.Inst.ChangeGameState(EGameState.Game);
        EventManager.TriggerEvent(ECutSceneEvent.EndStartCutScene);
        Sound.OnPlaySound(Sound.EAudioType.StartMainBGM);

        SetActiveThisObject();
    }

    private void SetActiveThisObject()
    {
        cutSceneCoverPanel.SetActive(false);
        Destroy(gameObject);
    }

}
