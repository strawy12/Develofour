using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FileSearchPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField searchField;

    [SerializeField]
    private List<WindowIcon> windowIconList;

    [SerializeField]
    private Button searchBtn;

    public void Init()
    {
        searchBtn.onClick.AddListener(SearchFile);
        searchField.onSubmit.AddListener(SearchFile);

        InitAllIcon();
    }

    public void Show()
    {
        gameObject.SetActive(true);

    }


    private void SearchFile(string text)
    {
        if (text.Length < 2)
        {
            return;
        }

        if (DataManager.Inst.GetIsStartTutorial(ETutorialType.Profiler) && !DataManager.Inst.GetIsClearTutorial(ETutorialType.Profiler))
        {
            if (text == "이름")
            {
                EventManager.TriggerEvent(ETutorialEvent.SearchNameText);
            }
        }


        List<FileSO> fileList = FileManager.Inst.ProfileSearchFile(text);
        ShowFileIcon(fileList);
    }

    private void SearchFile()
    {
        List<FileSO> fileList = FileManager.Inst.ProfileSearchFile(searchField.text);
        if (DataManager.Inst.GetIsStartTutorial(ETutorialType.Profiler) && !DataManager.Inst.GetIsClearTutorial(ETutorialType.Profiler))
        {
            if (searchField.text == "이름")
            {
                EventManager.TriggerEvent(ETutorialEvent.SearchNameText);
            }
        }

        ShowFileIcon(fileList);
    }

    private void ShowFileIcon(List<FileSO> fileList)
    {
        HideAllFile();
        Debug.Log("fileListCount: " + fileList.Count);
        if (fileList.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < fileList.Count; i++)
        {
            if (i >= 5) break;
            windowIconList[i].SetFileData(fileList[i]);
            windowIconList[i].gameObject.SetActive(true);
        }
    }
    private void InitAllIcon()
    {
        foreach (WindowIcon icon in windowIconList)
        {
            icon.Init();
            icon.gameObject.SetActive(false);
        }
    }
    private void HideAllFile()
    {
        foreach (WindowIcon icon in windowIconList)
        {
            icon.gameObject.SetActive(false);
        }
    }
    private void GuideInputPanel(object[] ps)
    {
        GuideUISystem.OnGuide?.Invoke(searchField.transform as RectTransform);
    }
}
