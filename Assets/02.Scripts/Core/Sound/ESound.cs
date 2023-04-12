using UnityEngine;

public partial class Sound : MonoBehaviour
{
    public enum EAudioType
    {
        None = -1,

        StartMainBGM = 0,
        AfterDiscordMail,
        ComputerNoise,
        InterrogationRoom,
        BGMEnd,

        Notice = 1001,
        PhoneCall,
        PhoneReceive,
        StartPC,
        RetroTyping,
        MonologueTyping,
        MouseDown,
        MouseUp,
        USBConnect,
        StartCutSceneScream,
        StartCutScenePoint,
        StartCutSceneLightPull,
        PhoneAlarm,
        End
    }

}

