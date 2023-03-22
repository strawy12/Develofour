using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextTrigger
{
    public static void CommandTrigger(string msg, GameObject obj = null)
    {

        string cmdMsg = EncordingCommandText(msg);
        string[] cmdMsgSplit = cmdMsg.Split('_');

        if (cmdMsgSplit.Length == 1)
        {
            return;
        }

        string cmdType = cmdMsgSplit[0];
        string cmdValue = cmdMsgSplit[1];

        switch (cmdType)
        {
            case "PS":
                {
                    Sound.EAudioType audioType = (Sound.EAudioType)Enum.Parse(typeof(Sound.EAudioType), cmdValue);
                    Sound.OnPlaySound?.Invoke(audioType);

                    break;
                }

            case "PSDL":
                {
                    Sound.EAudioType audioType = (Sound.EAudioType)Enum.Parse(typeof(Sound.EAudioType), cmdValue);
                    float? delayNull = Sound.OnPlaySound?.Invoke(audioType);
                    float delay = delayNull != null ? (float)delayNull : 0f;
                    EventManager.TriggerEvent(ETextboxEvent.Delay, new object[] { delay });
                    break;
                }

            case "SK":
                {
                    if(obj == null)
                    {
                        Debug.LogError("obj가 넘어오지 않았습니다.");
                    }
                    string[] cmdValueArray = cmdValue.Split(',');
                    float delay = float.Parse(cmdValueArray[0]);
                    float strength = float.Parse(cmdValueArray[1]);
                    int vibrato = int.Parse(cmdValueArray[2]);
                    EventManager.TriggerEvent(ETextboxEvent.Shake, new object[] { delay, strength, vibrato, obj });
                    break;
                }

            case "DL":
                {
                    float delay = float.Parse(cmdValue);
                    EventManager.TriggerEvent(ETextboxEvent.Delay, new object[] { delay });
                    break;
                }
            case "SN":
                {
                    string[] cmdValueArray = cmdValue.Split(',');
                    ENoticeType noticeTypeobj;
                    if (Enum.TryParse(cmdValueArray[0], out noticeTypeobj))
                    {
                        float delay = float.Parse(cmdValueArray[1]);

                        NoticeSystem.OnGeneratedNotice?.Invoke(noticeTypeobj, delay);
                    }
                    else
                    {

                    }
                    break;
                }
        }
    }



    // text에서 cmdText 뽑아냄
    // ex) {BS_WriterBGM}
    public static string EncordingCommandText(string message)
    {
        string richText = "";

        if (message[0] == '{')
        {
            message = message.Substring(1);
        }

        for (int i = 0; i < message.Length; i++)
        {

            if (message[i] == '}')
            {
                break;
            }
            richText += message[i];
        }

        return richText;
    }

    public static string RemoveCommandText(string message, Dictionary<int, Action> triggerDictionary, GameObject gobj, bool registerCmd = false)
    {

        string removeText = message;
        int signTextLength = 0;

        for (int i = 0; i < removeText.Length; i++)
        {
            if (i < 0)
            {
                i = 0;
            }
            if (removeText[i] == '{')
            {
                string signText = TextTrigger.EncordingCommandText(removeText.Substring(i)); // {} 문자열
                removeText = removeText.Remove(i, signText.Length + 2); // {} 이 문자열을 제외시킨 문자열

                if (registerCmd)
                {
                    if (triggerDictionary == null || gobj == null)
                    {
                        Debug.LogError("딕셔너리랑 게임오브젝트가 null임");
                    }
                    else
                    {
                        if (triggerDictionary.ContainsKey(i + signTextLength))
                        {
                            triggerDictionary[i + signTextLength] += () => TextTrigger.CommandTrigger(signText, gobj);
                        }
                        else
                        {
                            triggerDictionary.Add(i + signTextLength, () => TextTrigger.CommandTrigger(signText, gobj));
                        }
                    }
                }

                signTextLength += signText.Length;
                i -= signText.Length;
            }
        }

        return removeText;
    }
}
