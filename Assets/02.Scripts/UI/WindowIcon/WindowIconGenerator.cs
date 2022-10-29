using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowIconGenerator : MonoBehaviour
{
    [SerializeField]
    private WindowIconDataListSO windowIconDataList;
    private WindowIcon windowIconPrefab;

    private  WindowIcon[,] windowCellIcons = new WindowIcon[18, 9];

    private WindowIconData data;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        foreach(WindowIconData windowIconData in windowIconDataList.windowIconDataList)
        {
            WindowIcon icon = Instantiate(windowIconData.iconObject, transform);

            float x = (windowIconData.cellPoint.x * Constant.WINDOWICONSIZE.x) + Constant.WINDOWDEFAULTPOS.x;
            float y = (windowIconData.cellPoint.y * Constant.WINDOWICONSIZE.y) - Constant.WINDOWDEFAULTPOS.y;
            icon.Create(windowIconData);
            icon.rectTranstform.localPosition = new Vector3(x, y, icon.rectTranstform.localPosition.z);

            Debug.Log(windowIconData.cellPoint);
            windowCellIcons[windowIconData.cellPoint.x, windowIconData.cellPoint.y] = icon;
        }
    }
}
