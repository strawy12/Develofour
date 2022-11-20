using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class NewsCutScene : CutScene
{
    [SerializeField]
    private Canvas windowCanvas;

    [SerializeField]
    private TextBox textBox;

    [SerializeField]
    private List<int> printTextCntList;

    [Header("Glitch")]
    [SerializeField]
    private DigitalGlitch digitalGlitch;
    [SerializeField]
    private float glitchBeforeDelay;
    [SerializeField]
    private float glitchDuration;

    [SerializeField]
    private float glitchAfterDelay;
    [Space]


    #region NewsAnchor
    [Header("News Anchor")]
    [SerializeField]
    private NewsAnchor newsAnchor;

    #endregion

    [Header("Background")]
    [SerializeField]
    private Image backgroundImage;

    [Header("NewsScreen")]
    [SerializeField]
    private NewsScreen newsScreen;
    [SerializeField]
    private float newsScreenFadeDuration;

    [Header("StopIcon")]
    [SerializeField]
    private Image stopIconImage;
    [SerializeField]
    private float stopIconStartAlpha = 0.75f;
    [SerializeField]
    private float stopIconTargetSize = 1.5f;
    [SerializeField]
    private float stopIconDuration = 0.5f;

    [Header("CancelFullScreen")]
    [SerializeField]
    private float cancelFullScreenDelay = 2f;
    [SerializeField]
    private float cancelFullScreenOffsetY = -170f;

    [Header("PlayBar")]
    [SerializeField]
    private CanvasGroup playBar;
    [SerializeField]
    private float playBarDuration = 0.5f;

    private int anchorVoiceCnt = 0;
    private Queue<int> printTextCntQueue = new Queue<int>();

    protected override void StartCutScene()
    {
        printTextCntQueue.Clear();
        foreach (int cnt in printTextCntList)
        {
            printTextCntQueue.Enqueue(cnt);
        }

        base.StartCutScene();
    }

    protected override void ShowCutScene()
    {
        windowCanvas.enabled = false;
        textBox.Init(ETextDataType.News);
        StartCoroutine(NewsCoroutine());
    }

    private IEnumerator NewsCoroutine()
    {
        RectTransform rt = GetComponent<RectTransform>();
        Sound.OnPlayBGMSound?.Invoke(Sound.EBgm.NewsBGM);

        #region 시작 값
        digitalGlitch.Intensity = 1f;
        playBar.alpha = 0f;
        backgroundImage.color = Color.white;
        newsAnchor.canvasGroup.alpha = 1f;
        #endregion

        #region 글리치 효과 적용
        yield return new WaitForSeconds(glitchBeforeDelay);
        Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.Glitch);
        digitalGlitch.StartEffect(glitchDuration, false);
        yield return new WaitForSeconds(glitchDuration);
        yield return new WaitForSeconds(glitchAfterDelay);
        #endregion

        #region 뉴스 화면_1
        newsScreen.ChangeScreen(NewsScreen.ENewsScreenType.AIMurder, newsScreenFadeDuration);
        //yield return new WaitForSeconds(newsScreenFadeDuration);
        #endregion

        yield return PrintText();

        #region 뉴스 화면 2
        newsScreen.ChangeScreen(NewsScreen.ENewsScreenType.AIRegulation, newsScreenFadeDuration);
        yield return new WaitForSeconds(newsScreenFadeDuration);
        #endregion

        yield return PrintText();

        #region 일시정지 아이콘
        stopIconImage.rectTransform.localScale = Vector3.one;
        Color color = stopIconImage.color;
        color.a = stopIconStartAlpha;
        stopIconImage.color = color;
        stopIconImage.DOFade(0f, stopIconDuration);
        stopIconImage.rectTransform.DOScale(Vector3.one * stopIconTargetSize, stopIconDuration);
        playBar.DOFade(1f, playBarDuration);
        Sound.OnPlayEffectSound.Invoke(Sound.EEffect.SpaceKeyDown);
        yield return new WaitForSeconds(stopIconDuration);
        #endregion

        #region 전체화면 해제
        float delay = cancelFullScreenDelay / 2f;
        yield return new WaitForSeconds(delay);

        Vector2 pos = rt.anchoredPosition;
        pos.y = cancelFullScreenOffsetY;
        rt.anchoredPosition = pos;

        yield return new WaitForSeconds(delay);
        #endregion

        Sound.OnPlayEffectSound.Invoke(Sound.EEffect.EscKeyDown);
        EndCutScene();
        pos = rt.anchoredPosition;
        pos.y = 0f;
        rt.anchoredPosition = pos;
    }

    private void ShowNewsSceneNotice(float delay)
    {
        NoticeData data = new NoticeData();
        data.head = "AI 규제 법에 반대하기";
        data.body = "영상의 싫어요 버튼을 누름으로써 AI 규제 법에 대한 생각을 나타내세요";
        NoticeSystem.OnGeneratedNotice?.Invoke(data);
    }

    private IEnumerator PrintText()
    {
        textBox.ShowBox();

        int cnt = printTextCntQueue.Dequeue();

        for (int i = 0; i < cnt; i++)
        {
            yield return AnchorSpeak();
            if (i != cnt - 1)
            { yield return new WaitForSeconds(1f); }
        }
    }

    private IEnumerator AnchorSpeak()
    {
        float delay = -1f;
        if (Sound.OnPlayEffectSound != null)
        {
            delay = Sound.OnPlayEffectSound.Invoke(Sound.EEffect.NewsAnchorVoice_01 + (anchorVoiceCnt++));
        }
        if (delay == -1f) { yield break; }

        textBox.PrintText();

        newsAnchor.StartSpeak();

        yield return new WaitForSeconds(delay);
        newsAnchor.EndSpeak();
    }

    protected override void EndCutScene()
    {
        digitalGlitch.ImmediatelyStop();

        windowCanvas.enabled = true;
        Browser.OnOpenSite?.Invoke(ESiteLink.Youtube_News);
        Window.currentWindow.WindowMaximum();
        ShowNewsSceneNotice(1f);


        anchorVoiceCnt = 0;
        textBox.EndPrintText();
        newsScreen.Release();
        base.EndCutScene();
    }

}
