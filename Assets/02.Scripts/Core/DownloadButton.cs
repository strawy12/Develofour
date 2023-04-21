using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DownloadButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        FileManager.Inst.AddFile(downloadFile, "User\\C\\다운로드\\");
        string str = "C\\다운로드 폴더에서 확인해주세요.";
        NoticeSystem.OnNotice.Invoke(downloadFile.fileName + " 파일이 다운로드 되었습니다.", str, 0.1f, true, null, Color.white, ENoticeTag.None);
    }

    public void PointerEnter()
    {
        Debug.Log("다운로드 버튼 엔터");
        this.gameObject.SetActive(true);
    }

    public void PointerExit()
    {
        Debug.Log("다운로드 버튼 엔터");
        this.gameObject.SetActive(false);
    }
}
