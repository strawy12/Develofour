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

#if UNITY_EDITOR
    [ContextMenu("DebugTool")]
    public void DebugTool()
    {
        Init();
        foreach (TextTriggerData data in textTriggerList)
        {
            Debug.Log($"{data.text}�� ��ġ�� {text.text.IndexOf(data.text)}�Դϴ�");
        }
    } 
#endif

    public void SetTriggerPosition()
    {
        Define.SetTriggerPosition(text, textTriggerList);
    }
}
