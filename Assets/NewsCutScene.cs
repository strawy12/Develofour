using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private float bgFadeDuration;

    [SerializeField]
    private NewsScreen newsScreen;
    [SerializeField]
    private float newsScreenFadeDuration;

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
        yield break;

        //TODO 앵커가 다 말할 동안 대기를 해야하는데 이 대기 시간을 임의로 하지말고 
        // 텍스트박스가 텍스트 길이에 따른 시간을 반환 시켜줘야한다
        // 사운드 기준이면 사운드의 길이를 가져와야한다
        AnchorSpeak("많은 사람들이 충격에 휩싸였던 경호원 AI 오작동 살인 사건, 경호원 AI가 B를 구하기 위해 약에 취한 A씨를 죽인 사건이었는데요.");
        newsScreen.ChangeScreen(NewsScreen.ENewsScreenType.AIMurder);
        newsAnchor.EndSpeak();
        yield return new WaitForSeconds(1f);
        AnchorSpeak("]1");
        newsAnchor.EndSpeak();
        newsScreen.ChangeScreen(NewsScreen.ENewsScreenType.AIRegulation);
        yield return new WaitForSeconds(1f);
        AnchorSpeak("]1");

    }

    private void AnchorSpeak(string text)
    {
        /*TextBox(text)*/
        newsAnchor.StartSpeak();
    }

}
