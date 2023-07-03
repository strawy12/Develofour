using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField]
    private TMP_Text chatText;

    [SerializeField]
    private int maxCnt = 12;

    [SerializeField]
    private Vector2 offset;

    [ContextMenu("DDs")]
    public void SetChatText()
    {
        rectTransform =GetComponent<RectTransform>();
        string text = "광운도 하인쪽에 위치한 계곡에 도착했다. 짐 정리하고 계곡 가서 놀았다. 아.. 실수로 방학 보고서를 안 들고와서..";
        text = TextLineDown(text);
        chatText.SetText(text);
        chatText.ForceMeshUpdate();
        SetSize();
    }
    
    public void Init(string text)
    {
        text = TextLineDown(text);
        chatText.SetText(text);
        chatText.ForceMeshUpdate();
        SetSize();
    }

    private void SetSize()
    {
        Vector2 topLeft = new Vector2(int.MaxValue, 0);
        Vector2 bottomRight = new Vector2(0, int.MaxValue); 

        foreach (TMP_CharacterInfo info in chatText.textInfo.characterInfo)
        {
            if(info.topLeft.x < topLeft.x)
            {
                topLeft.x = info.topLeft.x;
            }

            if (info.topLeft.y > topLeft.y)
            {
                topLeft.y = info.topLeft.y;
            }

            if (info.bottomRight.x > bottomRight.x)
            {
                bottomRight.x = info.bottomRight.x;
            }

            if (info.bottomRight.y < bottomRight.y)
            {
                bottomRight.y = info.bottomRight.y;
            }
        }

        Vector2 newSize = new Vector2(bottomRight.x - topLeft.x, topLeft.y - bottomRight.y);
        Debug.Log(newSize);
        newSize += offset;
        rectTransform.sizeDelta = newSize;
    }

    private string TextLineDown(string text)
    {
        if(text.Length > maxCnt)
        {
            string newText = "";
            for(int i = 0; i < text.Length; i++)
            {
                newText += text[i];

                if(i >= maxCnt && i % maxCnt == 0)
                {
                    newText += "\n";
                }
            }

            text = newText;
        }
        return text;
    }
}
