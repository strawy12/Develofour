using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundIcons : MonoBehaviour
{
    [SerializeField]
    private WindowIcon iconPrefab;
    [SerializeField]
    private DirectorySO backgroundDirectory;

    private List<WindowIcon> iconList;

    private void Start()
    {
        IconListInit();

        EventManager.StartListening(ELibraryEvent.AddFile, RefreshIcons);
        RefreshIcons(null);
    }

    private void IconListInit()
    {
        iconList = new List<WindowIcon>();

        WindowIcon[] icons = GetComponentsInChildren<WindowIcon>();
        iconList.AddRange(icons);
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
        WindowIcon icon = Instantiate(iconPrefab, transform);
        icon.Init(true);
        icon.SetFileData(file);
        icon.name = file.name;
        icon.gameObject.SetActive(true);

        iconList.Add(icon);
    }

}
