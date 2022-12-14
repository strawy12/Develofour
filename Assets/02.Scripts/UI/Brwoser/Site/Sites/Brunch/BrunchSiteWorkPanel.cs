using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrunchSiteWorkPanel : MonoBehaviour
{
    private BrunchWorkDataSO workData;

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

    private int writeCount;
    public void Init(BrunchWorkDataSO workData)
    {
        this.workData = workData;
        SettingPanel();
    }

    private void SettingPanel()
    {
        workThumbnailTitleText.text = workData.titleText;
        titleNameText.text = workData.titleText;
        writeUserText.text = workData.userCnt.ToString();
        writeCount = workData.writeCnt;
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
