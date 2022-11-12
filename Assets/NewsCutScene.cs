using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct TransformData
{
    public Vector3 pos;
    public Quaternion rot;
}

public class NewsCutScene : CutScene
{
    [SerializeField]
    private Canvas windowCanvas;

    [SerializeField]
    private TextBox textBox;

    [SerializeField]
    private List<int> printTextCntList;

    [Space]
    #region NewsAnchor
    [Header("News Anchor")]
    [SerializeField]
    private NewsAnchor newsAnchor;
    [SerializeField]
    private float anchorFadeDuration;

    [SerializeField]
    private List<TransformData> anchorTrmList;

    private int anchorTrmIdx = 0;


    #endregion

    [Header("Background")]
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private float bgFadeDuration;

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

    protected override void ShowCutScene()
    {
        windowCanvas.enabled = false;
        textBox.Init(ETextDataType.News);
        StartCoroutine(NewsCoroutine());
    }

    private IEnumerator NewsCoroutine()
    {

        #region 시작 FadeIn
        backgroundImage.DOFade(0f, 0f);
        backgroundImage.DOFade(1f, bgFadeDuration);
        yield return new WaitForSeconds(bgFadeDuration);
        #endregion

        #region 앵커 움직임_1
        TransformData data = anchorTrmList[anchorTrmIdx++];
        newsAnchor.rectTransform.anchoredPosition = data.pos;
        newsAnchor.rectTransform.rotation = data.rot;
        #endregion

        #region 앵커 페이드 인
        newsAnchor.canvasGroup.DOFade(1f, anchorFadeDuration);
        yield return new WaitForSeconds(anchorFadeDuration);
        #endregion

        #region 앵커 움직임_2
        data = anchorTrmList[anchorTrmIdx++];

        newsAnchor.rectTransform.DORotate(data.rot.eulerAngles, newsScreenFadeDuration);
        newsAnchor.rectTransform.DOAnchorPos(data.pos, newsScreenFadeDuration);
        yield return new WaitForSeconds(newsScreenFadeDuration * 0.5f);
        #endregion

        #region 뉴스 화면_1
        newsScreen.ChangeScreen(NewsScreen.ENewsScreenType.AIMurder, newsScreenFadeDuration);
        yield return new WaitForSeconds(newsScreenFadeDuration);
        #endregion

        yield return PrintText();

        #region 뉴스 화면 2
        newsScreen.ChangeScreen(NewsScreen.ENewsScreenType.AIRegulation, newsScreenFadeDuration);
        yield return new WaitForSeconds(newsScreenFadeDuration);
        #endregion

        yield return PrintText();

        textBox.EndPrintText();
        #region 일시정지 아이콘
        stopIconImage.rectTransform.localScale = Vector3.one;
        Color color = stopIconImage.color;
        color.a = stopIconStartAlpha;
        stopIconImage.color = color;
        stopIconImage.DOFade(0f, stopIconDuration);
        stopIconImage.rectTransform.DOScale(Vector3.one * stopIconTargetSize, stopIconDuration);
        yield return new WaitForSeconds(stopIconDuration);
        #endregion

        // 키보드 사운드 넣기

        windowCanvas.enabled = true;
        Browser.OnOpenSite?.Invoke(ESiteLink.Youtube_News);
        EndCutScene();
    }



    private IEnumerator PrintText()
    {
        textBox.ShowBox();

        for (int i = 0; i < printTextCntList[0]; i++)
        {
            yield return AnchorSpeak();
            yield return new WaitForSeconds(1f);
        }

        printTextCntList.RemoveAt(0);
    }

    //TODO 앵커가 다 말할 동안 대기를 해야하는데 이 대기 시간을 임의로 하지말고 
    // 텍스트박스가 텍스트 길이에 따른 시간을 반환 시켜줘야한다
    // 사운드 기준이면 사운드의 길이를 가져와야한다
    private IEnumerator AnchorSpeak()
    {
        float delay = textBox.PrintText();
        newsAnchor.StartSpeak();

        yield return new WaitForSeconds(delay);
        newsAnchor.EndSpeak();
    }

}
