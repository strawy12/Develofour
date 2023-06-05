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
            return;
        }
        btn.Init();
    }
}
