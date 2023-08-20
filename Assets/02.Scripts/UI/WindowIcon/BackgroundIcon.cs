using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackgroundIcon : WindowIcon, ISelectable
{
    public Action OnSelected { get; set; }
    public Action OnUnSelected { get; set; }


    public bool IsSelected(GameObject hitObject)
    {
        bool flag1 = hitObject == gameObject;
        return isSelected && flag1;
    }
        
    protected override void Select() 
    {
        //base.Select();

        this?.SelectedIcon(true);
        WindowManager.Inst.SelectObject(this);
        isSelected = true;
    }
    protected override void UnSelect()
    {
        //base.UnSelect();
        this?.SelectedIcon(false);
        WindowManager.Inst.SelectedObjectNull();
        isSelected = false;
        clickCount = 0;
    }

}
