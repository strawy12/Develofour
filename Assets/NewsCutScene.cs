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

        //TODO ��Ŀ�� �� ���� ���� ��⸦ �ؾ��ϴµ� �� ��� �ð��� ���Ƿ� �������� 
        // �ؽ�Ʈ�ڽ��� �ؽ�Ʈ ���̿� ���� �ð��� ��ȯ ��������Ѵ�
        // ���� �����̸� ������ ���̸� �����;��Ѵ�
        AnchorSpeak("���� ������� ��ݿ� �۽ο��� ��ȣ�� AI ���۵� ���� ���, ��ȣ�� AI�� B�� ���ϱ� ���� �࿡ ���� A���� ���� ����̾��µ���.");
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
