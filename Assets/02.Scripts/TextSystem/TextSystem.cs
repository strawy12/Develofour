﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TextSystem : MonoBehaviour 
{
    protected Dictionary<int, Action> triggerDictionary;

    
    public virtual void Init()
    {

    }

    public virtual void Release()
    {

    }


    protected virtual void Start()
    {
        triggerDictionary = new Dictionary<int, Action>();
    }

    protected void CommandTrigger(string msg)
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
                    Debug.Log("와~");

                    break;
                }

            case "PSDL":
                {
                    Sound.EAudioType audioType = (Sound.EAudioType)Enum.Parse(typeof(Sound.EAudioType), cmdValue);
                    float? delayNull = Sound.OnPlaySound?.Invoke(audioType);
                    float delay = delayNull != null ? (float)delayNull : 0f;
                    SetDelay(delay);
                    break;
                }

            case "DL":
                {
                    float delay = float.Parse(cmdValue);
                    SetDelay(delay);
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


    public abstract void SetDelay(float value);

    // text에서 cmdText 뽑아냄
    // ex) {BS_WriterBGM}
    protected string EncordingCommandText(string message)
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

    // AI -> 이벤트 전부 한번에 실행
    // 텍스트박스는 인덱스에 맞춰 실행 
    protected string RemoveCommandText(string message, bool registerCmd = false)
    {
        string removeText = message;

        for (int i = 0; i < removeText.Length; i++)
        {
            if (i < 0)
            {
                i = 0;
            }
            if (removeText[i] == '{')
            {
                string signText = EncordingCommandText(removeText.Substring(i)); // {} 문자열
                removeText = removeText.Remove(i, signText.Length + 2); // {} 이 문자열을 제외시킨 문자열

                if (registerCmd)
                {
                    if (triggerDictionary.ContainsKey(i))
                    {
                        triggerDictionary[i] += () => CommandTrigger(signText);
                    }
                    else
                    {
                        triggerDictionary.Add(i, () => CommandTrigger(signText));
                    }
                }

                i -= signText.Length;
            }
        }

        return removeText;
    }
}
