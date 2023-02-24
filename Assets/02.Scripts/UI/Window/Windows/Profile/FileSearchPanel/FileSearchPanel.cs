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

    private void SearchFile(string text)
    {
        List<FileSO> fileList = FileManager.Inst.ProfileSearchFile(text);
        ShowFileIcon(fileList);    
    }

    private void SearchFile()
    {
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
}
