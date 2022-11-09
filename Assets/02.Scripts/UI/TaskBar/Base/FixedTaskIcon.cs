using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTaskIcon : TaskIcon
{
    [SerializeField]
    private Window windowPrefab;

    [SerializeField]
    private Transform windowParent;

    private void Awake()
    {
        isFixed = true;
        Init(windowPrefab);
    }

    protected override void LeftClick()
    {
        if(targetWindowList.Count != 0)
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
