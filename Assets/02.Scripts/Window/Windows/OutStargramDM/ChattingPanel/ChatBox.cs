using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    [SerializeField]
    private TMP_Text chatText;

    [SerializeField]
    private int maxCnt = 20;

    [SerializeField]
    private Vector2 offset;

    public void Setting(string text)
    {
        text = TextLineDown(text);
        chatText.SetText(text);
        chatText.ForceMeshUpdate();
        SetSize();
    }
    public void Release()
    {
        chatText.SetText("");
        //chatText.ForceMeshUpdate();
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
        newSize += offset;
        ((RectTransform)transform).sizeDelta = newSize;
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
