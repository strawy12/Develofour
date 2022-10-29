using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notepad : Window
{
    enum ETitle
    {
        None = -1,
        Default,
        Password,
        
    }
    private TMP_InputField inputField;

    [SerializeField]
    private ETitle eTitle;
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
        windowTitleID = (int)eTitle;
        OnSelected += inputField.ActivateInputField;
        OnUnSelected += () => inputField.DeactivateInputField();
    }


}
