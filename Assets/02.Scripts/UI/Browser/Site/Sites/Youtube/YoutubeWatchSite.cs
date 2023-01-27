using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class YoutubeWatchSite : Site
{
    [SerializeField] private YoutubeInteractionButton likeBtn;
    [SerializeField] private YoutubeInteractionButton hateBtn;

    [SerializeField]
    private List<Image> moreVideoImageList;

    private void Start()
    {
        hateBtn.OnClick.AddListener(ClickHateBtn);
    }

    private void ClickHateBtn()
    {
        EventManager.TriggerEvent(EYoutubeSiteEvent.ClickHateBtn);
        hateBtn.OnClick.RemoveListener(ClickHateBtn);
    }
}
