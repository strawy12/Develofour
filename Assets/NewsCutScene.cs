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

    [ContextMenu("StartCutScene")]
    protected override void ShowCutScene()
    {
        StartCoroutine(NewsCoroutine());

    }

    private IEnumerator NewsCoroutine()
    {
        backgroundImage.DOFade(0f, 0f);
        backgroundImage.DOFade(1f, bgFadeDuration);
        yield return new WaitForSeconds(bgFadeDuration);

        TransformData data = anchorTrmList[anchorTrmIdx++];
        newsAnchor.rectTransform.anchoredPosition = data.pos;
        newsAnchor.rectTransform.rotation = data.rot;

        newsAnchor.canvasGroup.DOFade(1f, anchorFadeDuration);
        yield return new WaitForSeconds(anchorFadeDuration);

        data = anchorTrmList[anchorTrmIdx++];

        newsAnchor.rectTransform.DORotate(data.rot.eulerAngles, newsScreenFadeDuration);
        newsAnchor.rectTransform.DOAnchorPos(data.pos, newsScreenFadeDuration);
        yield return new WaitForSeconds(newsScreenFadeDuration * 0.5f);
        newsScreen.ChangeScreen(NewsScreen.ENewsScreenType.AIMurder, newsScreenFadeDuration);
        yield return new WaitForSeconds(newsScreenFadeDuration);
        AnchorSpeak("1");
        yield return new WaitForSeconds(3f);
        newsAnchor.EndSpeak();
        yield return new WaitForSeconds(1f);
        AnchorSpeak("2");
        yield return new WaitForSeconds(3f);
        newsAnchor.EndSpeak();
        yield return new WaitForSeconds(1f);
        AnchorSpeak("3");
        newsScreen.ChangeScreen(NewsScreen.ENewsScreenType.AIRegulation, newsScreenFadeDuration);
        yield return new WaitForSeconds(3f);
        newsAnchor.EndSpeak();
        yield return new WaitForSeconds(1f);
        AnchorSpeak("4");
        yield return new WaitForSeconds(3f);
        newsAnchor.EndSpeak();
        yield return new WaitForSeconds(1f);
        AnchorSpeak("5");
        yield return new WaitForSeconds(3f);
        newsAnchor.EndSpeak();

        stopIconImage.rectTransform.localScale = Vector3.one;
        Color color = stopIconImage.color;
        color.a = stopIconStartAlpha;
        stopIconImage.color = color;
        stopIconImage.DOFade(0f, stopIconDuration);
        stopIconImage.rectTransform.DOScale(Vector3.one * stopIconTargetSize, stopIconDuration);
    }

    //TODO 앵커가 다 말할 동안 대기를 해야하는데 이 대기 시간을 임의로 하지말고 
    // 텍스트박스가 텍스트 길이에 따른 시간을 반환 시켜줘야한다
    // 사운드 기준이면 사운드의 길이를 가져와야한다
    private void AnchorSpeak(string text)
    {
        /*TextBox(text)*/
        newsAnchor.StartSpeak();
    }

}
