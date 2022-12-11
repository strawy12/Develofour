using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ExtenstionMethod;
using TMPro;
using UnityEngine.UI;

public class NewsBanner : MonoBehaviour
{
    [SerializeField]
    private NewsBannerText bannerTextTemp;
    [SerializeField]
    private TMP_Text timeText;

    private bool isBannerPlay = false;

    [SerializeField]
    private float newsBannerDuration;
    [SerializeField]
    private float bannerSpawnDelay;
    [SerializeField]
    private float turnOnDuration;

    private int bannerTextCnt = 0;


    private List<NewsBannerText> bannerTextList;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public void Init()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        bannerTextList = new List<NewsBannerText>();
    }

    public void SpawnBanner(string msg)
    {
        NewsBannerText bannerText = Instantiate(bannerTextTemp, transform);
        bannerText.transform.SetSiblingIndex(2);
        bannerText.gameObject.SetActive(true);

        bannerText.Init();
        bannerText.SetText(msg);

        Vector2 startTextPos = rectTransform.anchoredPosition;
        startTextPos.x = rectTransform.rect.width * 0.5f + bannerText.rectTransform.sizeDelta.x * 0.5f;
        startTextPos.y = bannerText.rectTransform.anchoredPosition.y;

        Vector2 endTextPos = rectTransform.anchoredPosition;
        endTextPos.x = -(rectTransform.rect.width * 0.5f + bannerText.rectTransform.sizeDelta.x * 0.5f);
        endTextPos.y = bannerText.rectTransform.anchoredPosition.y;

        bannerText.rectTransform.anchoredPosition = startTextPos;
        bannerText.rectTransform.DOAnchorPos(endTextPos, newsBannerDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            bannerTextList.Remove(bannerText);
            Destroy(bannerText.gameObject);
        });

        bannerTextList.Add(bannerText);
    }

    public void StartBanner(string[] msg)
    {
        isBannerPlay = true;
        StartCoroutine(BannerEffect(msg));
        StartCoroutine(SetTimeText());
    }

    // 이거 호출하면 배너 시작됨
    IEnumerator BannerEffect(string[] msgArr)
    {
        canvasGroup.DOFade(1f, turnOnDuration);

        while (isBannerPlay)
        {
            string msg = msgArr[bannerTextCnt];
            SpawnBanner(msgArr[bannerTextCnt]);

            yield return new WaitForSeconds(bannerSpawnDelay);

            bannerTextCnt++;
            if (bannerTextCnt >= 4)
            {
                bannerTextCnt = 0;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator SetTimeText()
    {
        int minute = 17;
        while(isBannerPlay)
        {
            timeText.SetText($"09:{minute.ToString()}");
            yield return new WaitForSeconds(60);
            minute++;
        }
    }

    public void BannelStop()
    {
        isBannerPlay = false;
        canvasGroup.DOKill();
        canvasGroup.alpha = 0f;

        bannerTextList.ForEach(x =>
        {
            x.rectTransform.DOKill();
            Destroy(x.gameObject);
        });
        bannerTextList.Clear();

        StopAllCoroutines();
    }
}
