using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;


public class StartCutScene : MonoBehaviour
{
    public static Action OnPlayCutScene { get; private set; }

    [SerializeField]
    private Image titleLogo;
    [SerializeField]
    private TMP_Text fictionText;
    [SerializeField]
    private Image interrogationRoomSprite;

    [SerializeField]
    private Image blackImagePanel;

    [SerializeField]
    private Image backgroundImagePanel;

    [SerializeField]
    private GameObject cutSceneCoverPanel;

    [SerializeField]
    private List<Image> cutSceneImages;

    [SerializeField]
    private FinderWindows finderWindows;

#if UNITY_EDITOR
    public bool isSkip = false;
#endif

    private void Awake()
    {
        OnPlayCutScene += ShowFiction;
    }

    private void ShowFiction()
    {
        OnPlayCutScene -= ShowFiction;
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
            StartCoroutine(FictionCor());
        }
    }

    private IEnumerator FictionCor()
    {
        GameManager.Inst.OnChangeGameState?.Invoke(EGameState.CutScene);
        fictionText.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(1f);
        fictionText.DOFade(0, 1f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(PlayCutSceneCoroutine());
    }


    private IEnumerator PlayCutSceneCoroutine()
    {
        float? delay = Sound.OnPlaySound?.Invoke(Sound.EAudioType.StartCutSceneBGM);
        yield return new WaitForSeconds(0.8f);

        StartScene_1();
    }

    #region StartScene_1

    private void StartScene_1()
    {
        string monologID = $"{Constant.MonologKey.STARTCUTSCENE_1}_{1}";
        MonologSystem.AddOnEndMonologEvent(monologID, () => StartCoroutine(ChangeImage_1_1()));
        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
    }

    private IEnumerator ChangeImage_1_1()
    {
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.WomanWalk);
        backgroundImagePanel = cutSceneImages[0];
        backgroundImagePanel.color = Define.FadeColor;
        yield return new WaitForSeconds(0.5f);

        backgroundImagePanel.DOFade(1f, 2f);
        float delayTime = 2.2f;
        yield return new WaitForSeconds(delayTime + 3);
        backgroundImagePanel.DOFade(0f, 2f);
        yield return new WaitForSeconds(delayTime);

        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.WomanWalk);

        string monologID = $"{Constant.MonologKey.STARTCUTSCENE_1}_{2}";
        MonologSystem.AddOnEndMonologEvent(monologID, () => StartCoroutine(ChangeImage_1_2()));
        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
    }

    private IEnumerator ChangeImage_1_2()
    {
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.ManWalk);
        backgroundImagePanel = cutSceneImages[1];
        backgroundImagePanel.color = Define.FadeColor;
        yield return new WaitForSeconds(0.5f);

        backgroundImagePanel.DOFade(1f, 2f);
        float delayTime = 2.2f;
        yield return new WaitForSeconds(delayTime + 3);
        backgroundImagePanel.DOFade(0f, 2f);
        yield return new WaitForSeconds(delayTime);

        backgroundImagePanel.color = Define.FadeColor;
        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.ManWalk);

        string monologID = $"{Constant.MonologKey.STARTCUTSCENE_1}_{3}";
        MonologSystem.AddOnEndMonologEvent(monologID, () => StartCoroutine(ChangeImage_1_3()));
        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
    }
    private IEnumerator ChangeImage_1_3()
    {
        float? delay = Sound.OnPlaySound?.Invoke(Sound.EAudioType.FastWalk);
        backgroundImagePanel = cutSceneImages[2];
        backgroundImagePanel.color = Define.FadeColor;
        backgroundImagePanel.rectTransform.anchoredPosition = new Vector2(960f, 0f);
        yield return new WaitForSeconds(delay != null ? (float)delay : 5f);
        backgroundImagePanel.DOFade(1f, 1f);
        backgroundImagePanel.rectTransform.DOAnchorPosX(-960f, 5f);
        yield return new WaitForSeconds(5f);
        // 대기

        string monologID = $"{Constant.MonologKey.STARTCUTSCENE_1}_{4}";
        MonologSystem.AddOnEndMonologEvent(monologID, EndScene_1);
        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
    }

    private void EndScene_1()
    {
        StartCoroutine(StartScene_2());
    }

    #endregion

    #region Other Scenes

    private IEnumerator StartScene_2()
    { 
        backgroundImagePanel.DOFade(0f, 3f);
        yield return new WaitForSeconds(3f);
        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.StartCutSceneBGM);
        Sound.OnPlaySound.Invoke(Sound.EAudioType.StartCutSceneLightPull);
        EventManager.TriggerEvent(ECoreEvent.OpenVolume, new object[] { true });
        backgroundImagePanel = cutSceneImages[3];
        backgroundImagePanel.color = Define.FadeColor;
        backgroundImagePanel.DOFade(1f, 1.5f);
        yield return new WaitForSeconds(1.5f);
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.InterrogationRoom);

        string monologID = $"{Constant.MonologKey.STARTCUTSCENE_2}";

        MonologSystem.AddOnEndMonologEvent(monologID, EndScene_2);
        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
    }

    private void EndScene_2()
    {
        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.InterrogationRoom);
        StartCoroutine(StartScene_3());
    }

    private IEnumerator StartScene_3()
    {
        backgroundImagePanel.DOFade(0f, 1.5f);
        yield return new WaitForSeconds(1.5f);

        finderWindows.Show(1.5f);
        yield return new WaitForSeconds(3f);

        finderWindows.OnCompleted += EndScene_3;
        finderWindows.StartScene();
    }
    public void EndScene_3()
    {
        if (finderWindows.gameObject.activeSelf)
        {
            finderWindows.Hide(1.5f);
        }

        StartCoroutine(StartLoadingCor());
    }
    #endregion


    private IEnumerator StartLoadingCor()
    {
        yield return new WaitForSeconds(1.5f);
        EventManager.TriggerEvent(ECoreEvent.OpenVolume, new object[] { false });
        blackImagePanel.gameObject.SetActive(true);
        blackImagePanel.DOFade(0f, 2f);
        yield return new WaitForSeconds(2f);
        EndRequestCutScene();
    }

    public void EndRequestCutScene()
    {
        DataManager.Inst.SaveData.isWatchStartCutScene = true;
        cutSceneCoverPanel.SetActive(false);
        GameManager.Inst.ChangeGameState(EGameState.Game);
        EventManager.TriggerEvent(ECutSceneEvent.EndStartCutScene);
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.StartMainBGM);
        DestroyThisObject();
    }

    private void DestroyThisObject()
    {
        cutSceneCoverPanel.SetActive(false);
        Destroy(gameObject);
    }

}
