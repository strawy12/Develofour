using UnityEngine;

public partial class Sound : MonoBehaviour
{
    public enum EAudioType
    {
        None = -1,

        StartMainBGM = 0,
        AfterDiscordMail,
        ComputerNoise, 
        BGMEnd,

        Notice = 1001,
        PhoneCall,
        StartPC,
        End
    }

}

