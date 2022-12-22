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

    #region CG
    [Header("CG")]
    [SerializeField]
    private Image cgImage;
    [SerializeField]
    private float cgFadeDuration = 0.75f;

    [SerializeField]
    private float screenFadeDuration = 2f;
    #endregion

    [SerializeField]
    private CutSceneAnimation cutSceneAnimation;

    private int characterVoiceCnt = 0;
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
        textBox.Init(ETextDataType.News, TextBox.ETextBoxType.Simple);
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
        newsAnchor.canvasGroup.alpha = 1f;
        newsReporter.image.ChangeImageAlpha(0f);
        currentNewsCharacter = newsAnchor;

        newsBackground.ChangeBackground(NewsBackground.EBackgroundType.AI_Regulation, false);
        #endregion

        #region 글리치 효과 적용
        Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.Glitch);
        yield return new WaitForSeconds(glitchBeforeDelay);
        digitalGlitch.StartEffect(glitchDuration, false);
        yield return new WaitForSeconds(glitchDuration);
        yield return new WaitForSeconds(glitchAfterDelay);
        #endregion

        newsTitle.Show(false, titleTextList[0]);

        #region 뉴스 화면_1
        newsScreen.ChangeScreen(NewsScreen.ENewsScreenType.AIMurder, newsScreenDuration);
        //yield return new WaitForSeconds(newsScreenFadeDuration);
        #endregion

        yield return PrintText(true);

        // 공윤선 기자 타임

        #region 화면 전환
        newsAnchor.canvasGroup.alpha = 0f;
        characterVoiceCnt = 0;
        newsTitle.Show(true, titleTextList[1]);
        delay = newsBackground.ChangeBackground(NewsBackground.EBackgroundType.AI_MurderCase, true);
        newsScreen.ChangeScreen(NewsScreen.ENewsScreenType.AIRegulation, newsScreenDuration);
        yield return new WaitForSeconds(newsScreenDuration);
        newsReporter.image.ChangeImageAlpha(1f);
        currentNewsCharacter = newsReporter;
        yield return new WaitForSeconds(delay);

        #endregion
        cutSceneAnimation.Play();
        yield return PrintText(true);

        // 국회 AI 규제 발의
        newsTitle.Show(true, titleTextList[2]);
        newsBackground.ChangeBackground(NewsBackground.EBackgroundType.Regulation_Initiative, true);
        newsScreen.ChangeScreen(NewsScreen.ENewsScreenType.AIRegulationPass, newsScreenDuration);
        yield return new WaitForSeconds(newsScreenDuration);

        yield return PrintText(true);
        newsBanner.BannelStop();

        // CG아트가 나온상태
        cgImage.DOFade(1f, cgFadeDuration);

        yield return PrintText(true);

        textBox.SetTextBoxType(TextBox.ETextBoxType.Simple);
        yield return PrintText(false);

        Sound.OnPlayEffectSound.Invoke(Sound.EEffect.EscKeyDown);
        
        cgImage.DOColor(Color.black, screenFadeDuration);
        yield return new WaitForSeconds(screenFadeDuration);

        EndCutScene();
    }

    private void ShowNewsSceneNotice()
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.EndNewsCutScene, noticeDelay);
    }

    private IEnumerator PrintText(bool isNews)
    {
        textBox.ShowBox();
        int cnt = printTextCntQueue.Dequeue();

        for (int i = 0; i < cnt; i++)
        {
            if(isNews)
            {
                yield return NewsPrintText();
            }

            else
            {
                yield return WriterPrintText();
            }

            if (i != cnt - 1)
            { 
                yield return new WaitForSeconds(1f); 
            }
        }
        textBox.HideBox();
    }

    private IEnumerator WriterPrintText()
    {
            float delay = textBox.PrintText();
            yield return new WaitForSeconds(delay+1f);
    }

    private IEnumerator NewsPrintText()
    {
        float delay = PlaySound();

        if (delay == -1f) { yield break; }

        currentNewsCharacter.StartSpeak();

        textBox.PrintText();


        yield return new WaitForSeconds(delay);
        currentNewsCharacter.EndSpeak();
    }

    private float PlaySound()
    {
        if (Sound.OnPlayEffectSound != null)
        {
            Sound.EEffect sound = currentNewsCharacter == newsAnchor ? Sound.EEffect.NewsAnchor_01 : Sound.EEffect.NewsReporter_01;

            return Sound.OnPlayEffectSound.Invoke(sound + (characterVoiceCnt++));
        }

        return 0f;
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

        characterVoiceCnt = 0;
        textBox.EndPrintText();
        newsScreen.Release();
        base.EndCutScene();
    }

}
