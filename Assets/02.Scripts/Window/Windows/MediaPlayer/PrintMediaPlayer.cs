using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class MediaPlayer : Window
{
    private List<float> delayList;

    private float allDelay;

    private int TimeToIndex(float time)
    {
        float temp = 0f;
        for (int i = 0; i < delayList.Count; i++)
        {
            temp += delayList[i];
            if (temp >= time)
            {
                return i;
            }
        }
        return delayList.Count - 1;
    }

    private void InitDelayList()
    {
        delayList = new List<float>();
        Dictionary<int, float> delayDictionary;
        notCommandString = RemoveCommandText(mediaPlayerData.textData, out delayDictionary);

        for (int i = 0; i < notCommandString.Length; i++)
        {
            char c = notCommandString[i];

            delayList.Add(mediaPlayerData.textPlaySpeed);

            if (delayDictionary.ContainsKey(i))
            {
                delayList[i] += delayDictionary[i];
            }

            if (c == '\n')
            {
                delayList[i] += mediaPlayerData.endlineDelay;
            }

            allDelay += delayList[i];
        }

        allDelay += 1f;
    }

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

    protected string RemoveCommandText(string message, out Dictionary<int, float> delayDictionary)
    {
        string removeText = message;
        delayDictionary = new Dictionary<int, float>();

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
                if (i >= removeText.Length)
                {
                    i -= 1;
                }
                delayDictionary.Add(i, float.Parse(signText));

                i -= signText.Length;
            }
        }

        return removeText;
    }
}
