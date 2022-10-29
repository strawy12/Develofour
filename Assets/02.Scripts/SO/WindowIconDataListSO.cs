using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WindowIconData
{
    public bool isUseCheck;
    public WindowIcon iconObject;
    public Vector2Int cellPoint;
}

[CreateAssetMenu(menuName = "SO/UI/Window/IconDataList")]
public class WindowIconDataListSO : ScriptableObject
{
    public List<WindowIconData> windowIconDataList;
}
