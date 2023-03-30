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

    private bool isGuide;

    public void Init()
    {
        searchBtn.onClick.AddListener(SearchFile);
        searchField.onSubmit.AddListener(SearchFile);

        InitAllIcon();

        EventManager.StartListening(EProfileSearchTutorialEvent.GuideSearchInputPanel, GuideInputPanel);
    }

    public void Show()
    {
        gameObject.SetActive(true);

    }


    private void SearchFile(string text)
    {
        if(text.Length < 2)
        {
            return;
        }
        if(isGuide)
        {
            if(text == "���б�")
            {
                isGuide = false;
                EventManager.TriggerEvent(EProfileSearchTutorialEvent.SearchNameText);
                EventManager.StopListening(EProfileSearchTutorialEvent.GuideSearchInputPanel, GuideInputPanel);
            }
        }
        List<FileSO> fileList = FileManager.Inst.ProfileSearchFile(text);
        ShowFileIcon(fileList);    
    }

    private void SearchFile()
    {
        if (searchField.text.Length < 2)
        {
            return;
        }
        List<FileSO> fileList = FileManager.Inst.ProfileSearchFile(searchField.text);
        ShowFileIcon(fileList);
    }

    private void ShowFileIcon(List<FileSO> fileList)
    {
        HideAllFile();
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
        foreach(WindowIcon icon in windowIconList)
        {
            icon.gameObject.SetActive(false);
        }
    }
    private void GuideInputPanel(object[] ps)
    {
        GuideUISystem.OnGuide?.Invoke(searchField.transform as RectTransform);
        isGuide = true;
    }
}
