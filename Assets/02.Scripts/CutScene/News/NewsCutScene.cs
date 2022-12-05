using DG.Tweening;
using ExtenstionMethod;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NewsCutScene : CutScene
{
    [SerializeField]
    private Canvas windowCanvas;

    [SerializeField]
    private TextBox textBox;

    [SerializeField]
    private List<int> printTextCntList;

    [SerializeField]
    private float noticeDelay = 0.5f;

    [SerializeField]
    private string[] newsBannerText;


    #region Glitch
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
    #endregion

    #region NewsTitle
    [Header("NewsTitle")]
    [SerializeField]
    private NewsTitle newsTitle;

    [SerializeField]
    private List<string> titleTextList;
    #endregion

    #region News Anchor
    [Header("News Anchor")]
    [SerializeField]
    private NewsAnchor newsAnchor;
    #endregion

    #region News Reporter
    [Header("News Reporter")]
    [SerializeField]
    private NewsReporter newsReporter;
    #endregion

    #region News Banner
    [Header("News Banner")]
    [SerializeField]
    private NewsBanner newsBanner;
    #endregion

    #region Background
    [Header("Background")]
    [SerializeField]
    private NewsBackground newsBackground;
    #endregion

    #region NewsScreen
    [Header("NewsScreen")]
    [SerializeField]
    private NewsScreen newsScreen;
    [SerializeField]
    private float newsScreenDuration;
    #endregion

    #region StopIcon
    [Header("StopIcon")]
    [SerializeField]
    private Image stopIconImage;
    [SerializeField]
    private float stopIconStartAlpha = 0.75f;
    [SerializeField]
    private float stopIconTargetSize = 1.5f;
    [SerializeField]
    private float stopIconDuration = 0.5f;
    #endregion

    #region CancelFullScreen
    [Header("CancelFullScreen")]
    [SerializeField]
    private float cancelFullScreenDelay = 2f;
    [SerializeField]
    private float cancelFullScreenOffsetY = -170f;
    #endregion

    #region PlayBar
    [Header("PlayBar")]
    [SerializeField]
    private CanvasGroup playBar;
    [SerializeField]
    private float playBarDuration = 0.5f;
    #endregion

    private int anchorVoiceCnt = 0;
    private Queue<int> printTextCntQueue = new Queue<int>();
    private NewsCharacter currentNewsCharacter;
    private RectTransform rectTransform;


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
        float delay = 0f;
        rectTransform ??= GetComponent<RectTransform>();
        Sound.OnPlayBGMSound?.Invoke(Sound.EBgm.NewsBGM);
        newsAnchor.Init();
        newsReporter.Init();
        newsBanner.Init();
        newsScreen.Init();

        newsBanner.StartBanner(newsBannerText);

        #region 시작 값
        digitalGlitch.Intensity = 1f;
        playBar.alpha = 0f;
        newsAnchor.canvasGroup.alpha = 1f;
        newsReporter.image.ChangeImageAlpha(0f);
        currentNewsCharacter = newsAnchor;

        newsBackground.ChangeBackground(NewsBackground.EBackgroundType.AI_Regulation, false);
        #endregion

        #region 글리치 효과 적용
        yield return new WaitForSeconds(glitchBeforeDelay);
        Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.Glitch);
        digitalGlitch.StartEffect(glitchDuration, false);
        yield return new WaitForSeconds(glitchDuration);
        yield return new WaitForSeconds(glitchAfterDelay);
        #endregion

        newsTitle.Show(titleTextList[0]);

        #region 뉴스 화면_1
        newsScreen.ChangeScreen(NewsScreen.ENewsScreenType.AIMurder, newsScreenDuration);
        //yield return new WaitForSeconds(newsScreenFadeDuration);
        #endregion

        yield return PrintText();

        #region 화면 전환
        newsAnchor.canvasGroup.alpha = 0f;
        newsTitle.Show(titleTextList[1]);
        delay = newsBackground.ChangeBackground(NewsBackground.EBackgroundType.AI_MurderCase, true);
        newsScreen.ChangeScreen(NewsScreen.ENewsScreenType.AIRegulation, newsScreenDuration);
        yield return new WaitForSeconds(newsScreenDuration);
        newsReporter.image.ChangeImageAlpha(1f);
        currentNewsCharacter = newsReporter;
        yield return new WaitForSeconds(delay);

        #endregion


        yield return PrintText();

        newsScreen.ChangeScreen(NewsScreen.ENewsScreenType.AIRegulationPass, newsScreenDuration);
        yield return new WaitForSeconds(newsScreenDuration);

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

        newsBanner.BannelStop();

        yield return new WaitForSeconds(1f);

        textBox.PrintText();

        yield return new WaitForSeconds(3f);

        Sound.OnPlayEffectSound.Invoke(Sound.EEffect.EscKeyDown);

        #region 전체화면 해제
        delay = cancelFullScreenDelay / 2f;
        yield return new WaitForSeconds(delay);
        rectTransform.anchoredPosition = rectTransform.anchoredPosition.ChangeValue(y: cancelFullScreenOffsetY);
        yield return new WaitForSeconds(delay);
        #endregion

        EndCutScene();


    }

    private void ShowNewsSceneNotice()
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.EndNewsCutScene, noticeDelay);
    }

    private IEnumerator PrintText()
    {
        textBox.ShowBox();

        int cnt = printTextCntQueue.Dequeue();

        for (int i = 0; i < cnt; i++)
        {
            yield return Speak();
            if (i != cnt - 1)
            { yield return new WaitForSeconds(1f); }
        }
    }

    private IEnumerator Speak()
    {
        float delay = -1f;
        if (Sound.OnPlayEffectSound != null)
        {
            delay = 3f;//Sound.OnPlayEffectSound.Invoke(Sound.EEffect.NewsAnchorVoice_01 + (anchorVoiceCnt++));
        }
        if (delay == -1f) { yield break; }

        textBox.PrintText();

        currentNewsCharacter.StartSpeak();

        yield return new WaitForSeconds(delay);
        currentNewsCharacter.EndSpeak();
    }

    protected override void EndCutScene()
    {
        digitalGlitch.ImmediatelyStop();
        newsBanner.BannelStop();

        Sound.OnPlayBGMSound?.Invoke(Sound.EBgm.WriterBGM);

        windowCanvas.enabled = true;
        EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { ESiteLink.Youtube_News, 0 });

        Window.currentWindow?.WindowMaximum();
        ShowNewsSceneNotice();

        rectTransform.anchoredPosition = rectTransform.anchoredPosition.ChangeValue(y: 0);

        anchorVoiceCnt = 0;
        textBox.EndPrintText();
        newsScreen.Release();
        base.EndCutScene();
    }

}
