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
        StartCutSceneBGM,
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
        FastWalk,
        StartCutScenePoint,
        StartCutSceneLightPull,
        PhoneAlarm,
        LockInfoTrigger,
        FindInfoTrigger,
        DoorLock,
        ThrowObject,
        DogBark,
        Kick,
        ExitDoor,
        DogWhine,
        WomanSigh,
        WomanBreath,
        FallenDownObject,
        WomanWalk,
        ManWalk,
        End
    }

}

