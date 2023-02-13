using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HarmonyMassegeMail : Mail
{
    [SerializeField]
    private ScrollRect sr;

    private bool playBGM = false;

    public async void SetContentPosition()
    {
        await Task.Delay(200);

        sr.verticalNormalizedPosition = 0;
    }

    public override void Init()
    {
        base.Init();
    }

    public override void ShowMail()
    {
        if (playBGM == false)
        {
            playBGM = true;
            Sound.OnPlayBGMSound(Sound.EBgm.AfterDiscordMail);
        }
        base.ShowMail();
    }

    public override void HideMail()
    {
        base.HideMail();
    }

    public override void DestroyMail()
    {
        base.DestroyMail();
    }
}
