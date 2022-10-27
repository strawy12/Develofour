using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public Action OnSelected { get; set; }
    public Action OnUnSelected { get; set; }
}
