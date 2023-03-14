using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class StartCutScene : MonoBehaviour
{
    public TextMeshProUGUI[] mainTexts; // 12개

    private int cnt = 0;

    private bool isPlaying;

    public LoadingIcon loadingIcon;
    public GameObject loadingText;

    public CanvasGroup group;

    public float loadingDuration = 1.5f;

    public CanvasGroup timeText;

    void Awake()
    {
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    private void Start()
    {
        if (DataManager.Inst.SaveData.isWatchStartCutScene)
        {
            EndCutScene();
            Destroy(this.gameObject);
        }
        else
        {
            EventManager.StartListening(ECoreEvent.EndDataLoading, PlayCutScene);
        }
    }

    private void PlayCutScene(object[] ps)
    {
        StartShowText();
        StartCoroutine(PlayNoiseSound());
        StartCoroutine( CutSceneStart());
        GameManager.Inst.ChangeGameState(EGameState.CutScene);
        Debug.Log("S를 누를시 스타트 컷씬이 스킵되는 코드가 있습니다.");
    }

    IEnumerator CutSceneStart()
    {
        isPlaying = true;
        yield return new WaitForSeconds(1f);
        timeText.DOFade(1, 1f);
        yield return new WaitForSeconds(3f);
        timeText.DOFade(0, 1f).OnComplete(() => timeText.gameObject.SetActive(false));
        yield return new WaitForSeconds(1.5f);
        isPlaying = false;
        ShowText();
    }

    private IEnumerator PlayNoiseSound()
    {
        float? time = Sound.OnPlaySound?.Invoke(Sound.EAudioType.StartPC);
        if (time == null)
        {
            Debug.LogError("time is Null!, I guess Sound.OnPlaySound == null");
            yield break;
        }

        yield return new WaitForSeconds(time.Value);
        Debug.Log(11);
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.ComputerNoise);
    }

    private void StartShowText()
    {
        //StartCoroutine(OnType(mainTexts[0], 0.1f, scripts[0]));

        EventManager.StartListening(EInputType.InputMouseDown, ShowText);
        InputManager.Inst.AddKeyInput(KeyCode.S, onKeyDown: EndCutScene);
        InputManager.Inst.AddKeyInput(KeyCode.Space, onKeyDown: ShowText);
    }


    private void ShowText()
    {
        if (isPlaying)
            return;
        isPlaying = true;
        switch(cnt)
        {
            case 0:
                StartTyping(mainTexts[cnt++], 0, true);
                break;

            case 1:
                StartTyping(mainTexts[cnt++], 0, true);
                break;

            case 2:
                StartTyping(mainTexts[cnt++], 0, true);
                break;

            case 3:
                StartTyping(mainTexts[cnt++], 0, false);
                break;

            case 4:
                StartTyping(mainTexts[cnt++], 0, false);
                break;

            case 5:
                StartTyping(mainTexts[cnt++], 0, false);
                break;

            case 6:
                StartTyping(mainTexts[cnt++], 0, false);
                break;

            case 7:
                StartTyping(mainTexts[cnt++], 0, false);
                break;

            case 8:
                StartTyping(mainTexts[cnt++], 0, true);
                break;

            case 9:
                StartTyping(mainTexts[cnt++], 0, true);
                break;

            case 10:
                StartTyping(mainTexts[cnt++], 0, true);
                break;

            case 11:
                StartTyping(mainTexts[cnt++], 0, false);
                break;

            case 12:
                EndCutScene();
                break;
        }

    }

    private void ShowText(object[] obj)
    {
        ShowText();
    }

    private void StartTyping(TextMeshProUGUI text, float time, bool autoNext, float waitTime = 1.5f)
    {
        StartCoroutine(OnType(text, time, autoNext));
    }

    IEnumerator OnType(TextMeshProUGUI text, float time, bool autoNext, float waitTime = 0.5f)
    {
        isPlaying = true;

        string say = text.text;
        text.text = string.Empty;
        float interval;
        if (time == 0)
        {
            interval = 0.04f;
        }
        else
        {
            interval = time / say.Length;
        }

        text.gameObject.SetActive(true);

        foreach (char letter in say)
        {
            text.text += letter;

            Sound.OnPlaySound?.Invoke(Sound.EAudioType.RetroTyping);
            yield return new WaitForSeconds(interval);
        }

        EndText();

        if (autoNext == true)
        {
            yield return new WaitForSeconds(waitTime);
            ShowText();
        }
        else
        {
            EndText();
        }
    }

    private void EndText()
    {
        isPlaying = false;
    }

    private void EventStop()
    {
        EventManager.StopListening(EInputType.InputMouseDown, ShowText);
        InputManager.Inst.RemoveKeyInput(KeyCode.Space, onKeyDown: ShowText);
        InputManager.Inst.RemoveKeyInput(KeyCode.S, onKeyDown: EndCutScene);
    }

    private void EndCutScene()
    {
        EventStop();

        foreach (var text in mainTexts)
        {
            text.gameObject.SetActive(false);
        }

        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.ComputerNoise);

        StartLoading();
    }

    private void StartLoading()
    {
        loadingIcon.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(true);
        loadingIcon.StartLoading(loadingDuration, EndLoading);
    }

    private void EndLoading()
    {
        //여기에서 로그인 풀기
        DataManager.Inst.SaveData.isWatchStartCutScene = true;
        GameManager.Inst.ChangeGameState(EGameState.Game);
        EventManager.TriggerEvent(ECutSceneEvent.EndStartCutScene);
        Sound.OnPlaySound(Sound.EAudioType.StartMainBGM);
        SetActiveThisObject();
    }

    public void SetActiveThisObject()
    {
        Destroy(this.gameObject);
    }

}
