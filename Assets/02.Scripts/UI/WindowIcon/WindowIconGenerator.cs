using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowIconGenerator : MonoBehaviour
{
    [SerializeField]
    WindowIconDataListSO windowIconDataList;
    [SerializeField]
    WindowIcon windowIconPrefab;

    WindowIcon[,] windowCellIcons = new WindowIcon[18, 9];



    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        foreach(WindowIconData windowIconData in windowIconDataList.windowIconDataList)
        {
            WindowIcon icon = Instantiate(windowIconPrefab, transform);
            icon.Create(windowIconData);

            windowCellIcons[windowIconData.cellPoint.x, windowIconData.cellPoint.y] = icon;
        }
    }
}
