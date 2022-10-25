using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedTaskIcon : TaskIcon
{
    [SerializeField]
    private Window prefab;

    private void Awake()
    {
        isFixed = true;
        Init();

        windowTitle = prefab.WindowData.Title;
    }

    protected override void OpenTargetWindow()
    {
        if(targetWindowList.Count == 0)
        {
            CreateWindow();
        }

        base.OpenTargetWindow();
    }

    public void CreateWindow()
    {
        Window target = UIManager.Inst.CreateWindow(prefab.gameObject.name);
        target.CreateWindow();
    }

}
