using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextButton : MonoBehaviour
{
    [SerializeField]
    private Button currentBtn;
    [SerializeField]
    private TMP_Text buttonText;

    public Button CurrentBtn => currentBtn;
    public UnityEvent onClick => currentBtn.onClick;
    public string text
    {
        get => buttonText.text;
        set => buttonText.SetText(value);
    }

    public bool interactable
    {
        get => currentBtn.interactable;
        set => currentBtn.interactable = value;
    }

    public void SetText(string text)
    {
        buttonText.SetText(text);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        currentBtn ??= GetComponent<Button>();
        buttonText ??= GetComponentInChildren<TMP_Text>();
    }
#endif
}
