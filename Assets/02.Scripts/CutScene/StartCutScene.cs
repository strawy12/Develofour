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

    private bool isPlaying;
    private bool isEnd;

    public GameObject loadingSpr;
    public GameObject loadingText;

    public CanvasGroup group;

    void Awake()
    {
        group.alpha = 1;        
    }

    void Start()
    {
        StartCoroutine(OnType(mainTexts[0], 0.1f, scripts[0]));
        Flicker(underBarText);

        Debug.Log("S를 누를시 스타트 컷씬이 스킵되는 코드가 있습니다.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Destroy(this.gameObject);
        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isPlaying)
                return;
            ShowText();
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
        if (cnt != scripts.Length - 1)
        {
            StartCoroutine(OnType(mainTexts[cnt], 0.1f, scripts[cnt]));
        }
        else
        {
            EndCutScene();
        }
    }


    IEnumerator OnType(TextMeshProUGUI text, float interval, string Say)
    {
        isPlaying = true;
        foreach (char item in Say)
        {
            text.text += item;
            Vector3 pos = mainTexts[cnt].rectTransform.anchoredPosition;
            yield return new WaitForSeconds(interval);
            underBarText.rectTransform.anchoredPosition = new Vector3(mainTexts[cnt].rectTransform.rect.width + 150, pos.y, pos.z);
        }
        EndText();
    }

    private void EndText()
    {
        mainTexts[++cnt].text += ">> ";
        Vector3 pos = mainTexts[cnt].rectTransform.anchoredPosition;
        underBarText.rectTransform.anchoredPosition = new Vector3(mainTexts[cnt].rectTransform.rect.width + 150, pos.y, pos.z);
        isPlaying = false;
    }

    private void EndCutScene()
    {
        Debug.Log("와");
        isEnd = true;
        StopCoroutine(OnFlicker(underBarText));
        foreach (var text in mainTexts)
        {
            text.gameObject.SetActive(false);
        }
        underBarText.gameObject.SetActive(false);

        loadingSpr.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(true);
        loadingSpr.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -1080), 3f).OnComplete(() => { SetActiveThisObject(); });
    }

    public void SetActiveThisObject()
    {
        Destroy(this.gameObject);
    }
}
