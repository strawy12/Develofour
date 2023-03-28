using System;
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

                    break;
                }

            case "PSDL":
                {
                    Sound.EAudioType audioType = (Sound.EAudioType)Enum.Parse(typeof(Sound.EAudioType), cmdValue);
                    float? delayNull = Sound.OnPlaySound?.Invoke(audioType);
                    float delay = delayNull != null ? (float)delayNull : 0f;

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



    // text���� cmdText �̾Ƴ�
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

    // AI -> �̺�Ʈ ���� �ѹ��� ����
    // �ؽ�Ʈ�ڽ��� �ε����� ���� ���� 

    protected string RemoveCommandText(string message, bool registerCmd = false)
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
                string signText = EncordingCommandText(removeText.Substring(i)); // {} ���ڿ�
                removeText = removeText.Remove(i, signText.Length + 2); // {} �� ���ڿ��� ���ܽ�Ų ���ڿ�

                if (registerCmd)
                {
                    if (triggerDictionary.ContainsKey(i + signTextLength))
                    {
                        triggerDictionary[i + signTextLength] += () => CommandTrigger(signText);
                    }
                    else
                    {
                        triggerDictionary.Add(i + signTextLength, () => CommandTrigger(signText));
                    }
                }

                signTextLength += signText.Length;
                i -= signText.Length;
            }
        }

        return removeText;
    }
}
