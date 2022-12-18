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

    public void Init(BrunchWorkDataSO workData)
    {
        this.workData = workData;
        SettingPanel();
    }

    private void SettingPanel()
    {
        workThumbnailTitleText.text = workData.titleName.ToString();
        titleNameText.text = workData.titleName.ToString();
        writeUserText.text = workData.userCnt.ToString();
        writeCountText.text = workData.writeCnt.ToString();
        workThumbnailImage.sprite = workData.workSprite;
    }

    public void DiminishWriteCnt()
    {
        workData.writeCnt -= 1;
        writeCountText.text = workData.writeCnt.ToString();
        if (workData.writeCnt <= 0)
        {
            workData.writeCnt = 0;
        }
    }

}
