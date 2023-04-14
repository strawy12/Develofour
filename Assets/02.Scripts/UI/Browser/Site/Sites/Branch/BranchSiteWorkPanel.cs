using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BranchSiteWorkPanel: MonoBehaviour
{
    private BranchWorkDataSO workData;

    [SerializeField]
    private TMP_Text titleNameText;
    [SerializeField]
    private TMP_Text writeUserText;
    [SerializeField]
    private TMP_Text writeCountText;
    [SerializeField]
    private Image workThumbnailImage;
    [SerializeField]
    private TMP_Text workThumbnailTitleText;
    [SerializeField]
    private BranchThumbnailPanel thumbnailPanel;

    private int writeCount;
    public void Init(BranchWorkDataSO workData)
    {
        this.workData = workData;
        thumbnailPanel.OnClick += ClickPanel;
        SettingPanel();

    }

    private void ClickPanel()
    {
        EventManager.TriggerEvent(EBranchEvent.ShowPostList, new object[1] { workData });
    }

    private void SettingPanel()
    {
        workThumbnailTitleText.text = workData.titleText;
        titleNameText.text = workData.titleText;
        writeUserText.text = workData.userCnt.ToString();
        writeCount = workData.postDataList.Count;
        writeCountText.text = writeCount.ToString();
        workThumbnailImage.sprite = workData.workSprite;
    }

    public void DiminishWriteCnt()
    {
        writeCount -= 1;
        writeCountText.text = writeCount.ToString();
        if (writeCount <= 0)
        {
            writeCount = 0;
        }
    }

}
