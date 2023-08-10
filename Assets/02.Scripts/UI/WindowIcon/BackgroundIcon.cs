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
        Debug.Log("Select");
        WindowManager.Inst.SelectObject(this);
    }
    protected override void UnSelect()
    {
        Debug.Log("Unselect");
        WindowManager.Inst.SelectedObjectNull();
    }

}
