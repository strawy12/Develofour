using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotepadBody : MonoBehaviour
{
    public TMP_InputField inputField { get; private set; }
    private TMP_Text text;

    public RectTransform rectTransform => transform as RectTransform;

    [SerializeField] private List<TextTriggerData> textTriggerList;

    public void Init()
    {
        inputField = GetComponent<TMP_InputField>();
        text = inputField.textComponent;

    }

    [ContextMenu("DebugTool")]
    public void DebugTool()
    {
        Init();
        foreach (TextTriggerData data in textTriggerList)
        {
            Debug.Log($"{data.text}의 위치는 {text.text.IndexOf(data.text)}입니다");
        }
    }

    public void SetTriggerPosition()
    {
        TMP_CharacterInfo charInfo;
        text.ForceMeshUpdate();
        if (textTriggerList != null && textTriggerList.Count > 0)
        { 
            foreach (TextTriggerData trigger in textTriggerList)
            {
                charInfo = text.textInfo.characterInfo[trigger.id];
                (trigger.trigger.transform as RectTransform).anchoredPosition = charInfo.topLeft;
            }
        }
    }
}
