using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTaskIcon : TaskIcon
{
    [SerializeField]
    private Window windowPrefab;
    public Window WindowPrefab { get { return windowPrefab; } }
    [SerializeField]
    private Transform windowParent;

    private void Awake()
    {
        isFixed = true;
        Init((int)windowPrefab.WindowData.windowType);
    }

    protected override void LeftClick()
    {
        if (windowList.Count != 0)
        {
            base.LeftClick();
        }
        else
        {
            Window instWindow = Instantiate(windowPrefab, windowParent);
            AddWindow(instWindow);
        }

    }
}
