using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notepad : Window
{
    private TMP_InputField inputField;
    public string Text
    {
        get => inputField.text;
        set => inputField.text = value;
    }


    protected override void Bind()
    {
        base.Bind();
        inputField = transform.Find("InputField").GetComponent<TMP_InputField>();
        inputField.onSelect.AddListener((s) => SelectWindow());
    }

    public override void Init()
    {
        base.Init();
        OnSelected += inputField.ActivateInputField;
        OnUnSelected += () => inputField.DeactivateInputField();
    }


}
