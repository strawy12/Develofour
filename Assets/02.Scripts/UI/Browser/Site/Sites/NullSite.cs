using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NullSite : Site
{
    public TMP_Text errorText;

    public override void Init()
    {
        base.Init();
    }

    public void SetErrorText(string str)
    {
        errorText.text = str + "에 오타가 있는지 확인하세요.";
    }
}
