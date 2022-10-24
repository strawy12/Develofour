using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedTaskIcon : TaskIcon
{
    [SerializeField]
    private string windowPrefabName;

    private void Awake()
    {
        isFixed = true;
        targetWindowList = new List<Window>();

        Bind();
    }

    protected override void OpenTargetWindow()
    {
        if(targetWindowList.Count == 0)
        {
            Window target = UIManager.Inst.CreateWindow(windowPrefabName);
            SetTargetWindow(target);
        }

        base.OpenTargetWindow();
    }

}
