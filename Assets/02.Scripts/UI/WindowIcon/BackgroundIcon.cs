using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundIcon : WindowIcon, ISelectable
{
    public Action OnSelected;
    public Action OnUnSelected; 

    public bool IsSelected(GameObject hitObject)
    {
        if ()
            return IsSelected;
    }

    protected override void Select()
    {
        WindowManager.Inst.SelectObject(this);
    }
    protected override void UnSelect()
    {
        WindowManager.Inst.SelectedObjectNull();
    }
}
