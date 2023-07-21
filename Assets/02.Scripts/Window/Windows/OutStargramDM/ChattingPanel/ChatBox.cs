using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    
    public TMP_Text ChatText { get => chatText; }

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
        Debug.Log("SetSize");

        if (chatText.textInfo == null)
        {
            Debug.Log("SetSize textinfo is null");
            return;
        }

        Vector2 newSize = Define.CalcTextMaxSize(0,chatText.textInfo.characterInfo.Length - 1, chatText);

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
