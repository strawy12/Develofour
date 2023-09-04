using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DownloadButton : MonoBehaviour
{
    public FileSO downloadFile;

    public Button downloadBtn;
    private bool isInit;

    void OnEnable()
    {
        if(!isInit)
        {
            isInit = true;
            downloadBtn.onClick.AddListener(DownloadFile);
        }

    }
    public void DownloadFile()
    {
        FileManager.Inst.AddFile(downloadFile, Constant.FileID.DOWNLOAD);
    }


}
