using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : SOParent
{
    public string str;
    public int iValue;
    public List<bool> boolList = new List<bool>();

    public override void Setting(string[] ps)
    {
        str = (string)ps[2];
        iValue = int.Parse(ps[1]);
        //생성이 안되있을땐 안됨
        boolList.Clear();
        string[] listStr = ps[3].Split('/');
        for (int i = 0; i < listStr.Length; i++)
        {
            boolList.Add(bool.Parse(listStr[i]));
        }
    }
}
