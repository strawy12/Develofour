using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : SOParent
{
    public string str;
    public int iValue;

    public override void Setting(string[] ps)
    {
        str = (string)ps[0];
        iValue = int.Parse(ps[1]);
    }
}
