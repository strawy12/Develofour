using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotePad : Window
{
    [SerializeField]
    private TMP_InputField inputField;

    protected override void Init()
    {
        base.Init();
        inputField.onSelect.AddListener((s) => SelectWindow());

        OnSelected += inputField.ActivateInputField;
        OnUnSelected += () => inputField.DeactivateInputField();
    }

}
