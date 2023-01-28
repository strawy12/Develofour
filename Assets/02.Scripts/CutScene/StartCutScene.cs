using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class StartCutScene : MonoBehaviour
{
    public TextMeshProUGUI[] mainTexts;
    public TextMeshProUGUI underBarText;

    public string[] scripts;

    private int cnt = 0;
    public int underBarWidth ;

    private bool isPlaying;
    private bool isEnd;

    public GameObject loadingImage;
    public GameObject loadingText;

    public CanvasGroup group;

    public float loadingDuration = 0.5f;

    void Awake()
    {
        group.alpha = 1;        
    }

    void Start()
    {
        CutSceneStart();
        Debug.Log("S를 누를시 스타트 컷씬이 스킵되는 코드가 있습니다.");
    }

    void CutSceneStart()
    {
        EventManager.StartListening(EInputType.InputMouseDown, ShowText);
        InputManager.Inst.AddKeyInput(KeyCode.Space, onKeyDown: ShowText);
        StartCoroutine(OnType(mainTexts[0], 0.1f, scripts[0]));
        Flicker(underBarText);
        GameManager.Inst.ChangeGameState(EGameState.CutScene);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameManager.Inst.ChangeGameState(EGameState.Game);
            EndCutScene();
        }
    }

    public void Flicker(TextMeshProUGUI underBarText)
    {
        StartCoroutine(OnFlicker(underBarText));
    }

    private IEnumerator OnFlicker(TextMeshProUGUI underBarText)
    {
        while(!isEnd)
        {
            underBarText.color = new Color32(255, 255, 255, 255);
            yield return new WaitForSeconds(0.15f);
            underBarText.color = new Color32(255, 255, 255, 0);
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void ShowText()
    {
        if (isPlaying)
            return;

        if (cnt != scripts.Length - 1)
        {
            StartCoroutine(OnType(mainTexts[cnt], 0.1f, scripts[cnt]));
        }
        else
        {
            EndCutScene();
        }
    }

    private void ShowText(object[] obj)
    {
        ShowText();
    }

    IEnumerator OnType(TextMeshProUGUI text, float interval, string Say)
    {
        isPlaying = true;
        foreach (char item in Say)
        {
            text.text += item;
            Vector3 pos = mainTexts[cnt].rectTransform.anchoredPosition;
            yield return new WaitForSeconds(interval);
            underBarText.rectTransform.anchoredPosition = new Vector3(mainTexts[cnt].rectTransform.rect.width + underBarWidth, pos.y, pos.z);
        }
        EndText();
    }

    private void EndText()
    {
        mainTexts[++cnt].text += ">> ";
        Vector3 pos = mainTexts[cnt].rectTransform.anchoredPosition;
        underBarText.rectTransform.anchoredPosition = new Vector3(mainTexts[cnt].rectTransform.rect.width + underBarWidth, pos.y, pos.z);
        isPlaying = false;
    }

    private void EndCutScene()
    {
        Debug.Log("현재 로딩 시간 0.5초 나중에 수정");
        isEnd = true;
        EventManager.StopListening(EInputType.InputMouseDown, ShowText);
        InputManager.Inst.RemoveKeyInput(KeyCode.Space, onKeyDown: ShowText);
        StopCoroutine(OnFlicker(underBarText));
        foreach (var text in mainTexts)
        {
            text.gameObject.SetActive(false);
        }
        underBarText.gameObject.SetActive(false);
        loadingImage.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(true);
        loadingImage.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -1080), loadingDuration).OnComplete(() => 
        { 
            SetActiveThisObject();
            GameManager.Inst.ChangeGameState(EGameState.Game);
        });
    }

    public void SetActiveThisObject()
    {
        Destroy(this.gameObject);
    }
}
