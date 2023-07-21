using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserChatBoxPanel : MonoBehaviour
{
    [SerializeField]
    private ChatBox chatBoxTemp;

    [SerializeField]
    private RectTransform boxParent;
    [SerializeField]
    private ClickInfoTrigger clickInfoTriggerTemp;

    [SerializeField]
    private float offset = 5f;
    [SerializeField]
    private float spacing;

    private List<ChatBox> chatBoxList;
    public bool isMine;

#if UNITY_EDITOR

    public void AddChatBox(OutStarChatDataSO data)
    {
        if(chatBoxList == null)
        {
            chatBoxList = new List<ChatBox>();
        }
        ChatBox chatBox = Instantiate(chatBoxTemp, boxParent);
        chatBox.gameObject.SetActive(true);
        List<TextTriggerData> triggerDataList = new List<TextTriggerData>();

        if(data.outStarTriggerList != null && data.outStarTriggerList.Count != 0)
        {
            foreach (var outStarTrigger in data.outStarTriggerList)
            {
                TriggerDataSO triggerData = Define.GuidsToList<TriggerDataSO>("t:TriggerDataSO").Find(x => x.id == outStarTrigger.triggerID);
                if (triggerData != null)
                {
                    ClickInfoTrigger clickInfoTrigger = Instantiate(clickInfoTriggerTemp, chatBox.ChatText.transform);
                    RectTransform triggerRect = (RectTransform)clickInfoTrigger.transform;
                    triggerRect.anchorMin = new Vector2(0, 0.5f);
                    triggerRect.anchorMax = new Vector2(0, 0.5f);
                    triggerRect.pivot = new Vector2(0, 1f);
                    TextTriggerData textTriggerData = new TextTriggerData() { startIdx = outStarTrigger.startIdx, trigger = clickInfoTrigger, endIdx = outStarTrigger.endIdx };
                    clickInfoTrigger.Setting(triggerData);
                    clickInfoTrigger.gameObject.SetActive(true);
                    triggerDataList.Add(textTriggerData);
                }
            }
        }
        chatBoxList.Add(chatBox);
        chatBox.Setting(data.chatText);
        
        Define.SetTriggerPosition(chatBox.ChatText, triggerDataList);
        Define.SetTiggerSize(chatBox.ChatText, triggerDataList);

        SetSize();
    }

    public void SetSize()
    {
        float totalY = offset - spacing;
        foreach (var box in chatBoxList)
        {
            RectTransform boxRectTrm = (RectTransform)box.transform;
            totalY += boxRectTrm.sizeDelta.y + spacing;
        }
        boxParent.sizeDelta = new Vector2(boxParent.sizeDelta.x, totalY);
        RectTransform rectTransform = (RectTransform)transform;
        rectTransform.sizeDelta = boxParent.sizeDelta;
    }
#endif
}
