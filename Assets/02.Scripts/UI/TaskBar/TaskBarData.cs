using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ETaskBarOpenType
{
    Open,
    CreateOrigin,
    Clone,
} 


[Serializable]
public class TaskBarData
{
    public ETaskBarOpenType openType;
}
