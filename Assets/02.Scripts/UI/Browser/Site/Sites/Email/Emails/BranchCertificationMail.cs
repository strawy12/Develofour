using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchCertificationMail : Mail
{
    public HyperlinkButton btn;

    public override void Init()
    {
        base.Init();
        if(btn == null)
        {
            Debug.Log("�������Ʊ׾����Ʊడ������");
            return;
        }
        btn.Init();
    }
}
