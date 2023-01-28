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

    private List<BackgroundIcon> iconList;

    private void Start()
    {
        IconListInit();

        EventManager.StartListening(ELibraryEvent.AddFile, RefreshIcons);
        RefreshIcons(null);
    }

    private void IconListInit()
    {
        iconList = new List<BackgroundIcon>();

        BackgroundIcon[] icons = GetComponentsInChildren<BackgroundIcon>();
        iconList.AddRange(icons);
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
        BackgroundIcon icon = Instantiate(iconPrefab, transform);
        icon.Init(true);
        Debug.Log(file);
        icon.SetFileData(file);
        icon.name = file.name;
        icon.gameObject.SetActive(true);

        iconList.Add(icon);
    }

}
