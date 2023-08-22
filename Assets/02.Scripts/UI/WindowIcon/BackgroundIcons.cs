using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundIcons : MonoBehaviour
{
    [SerializeField]
    private BackgroundIcon iconPrefab;
    [SerializeField]
    private DirectorySO backgroundDirectory;
    [SerializeField]
    private BackgroundIcon libraryIcon;
    private List<BackgroundIcon> iconList;



    private void Start()
    {
        GameManager.Inst.OnStartCallback += Init;
    }
    public void Init()
    {
        IconListInit();

        EventManager.StartListening(ELibraryEvent.AddFile, RefreshIcons);
        RefreshIcons(null);
    }
    private void IconListInit()
    {
        if(iconList != null)
        {
            int cnt = 0;
            while(iconList.Count != 0)
            {
                cnt++;
                if(cnt > 100)
                {
                    Debug.LogWarning("while ¹® ¹Ýº¹");
                    break;
                }
                BackgroundIcon icon = iconList[0];
                iconList.RemoveAt(0);
                Destroy(icon.gameObject);
            }
        }

        iconList = new List<BackgroundIcon>();
        iconList.ForEach(x => x.Init(true));
    }

    private void RefreshIcons(object[] ep)
    {
        var newList = backgroundDirectory.children.Except(iconList.Select(x => x.File).ToList()).ToList();
        foreach (var file in newList)
        {
            CreateIcon(file);
        }
    }

    private void CreateIcon(FileSO file)
    {
        if(file == null)
        {
            return;
        }
        BackgroundIcon icon = Instantiate(iconPrefab, transform);
        icon.Init(true);
        icon.SetFileData(file);
        icon.name = file.name;
        
        icon.gameObject.SetActive(true);

        iconList.Add(icon);
    }

}
