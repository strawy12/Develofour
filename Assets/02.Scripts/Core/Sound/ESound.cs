using UnityEngine;

public partial class Sound : MonoBehaviour
{

    public enum EBgm
    {
        None = -1,
        StartBGM = 0,

        AfterDiscordMail,
        End
    }

    public enum EEffect 
    {
        None = -1,
        Notice = 1001,

        MouseClickDown,
        MouseClickUp,
        PhoneCall,
        End
    }

}

