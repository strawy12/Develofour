using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotepadBody : MonoBehaviour
{
    public TMP_InputField inputField { get; private set; }

    public RectTransform rectTransform => transform as RectTransform;

    public void Init()
    {
        inputField = GetComponent<TMP_InputField>();
    }
}
