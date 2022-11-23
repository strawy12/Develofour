using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotePad : Window
{
    [SerializeField]
    private TMP_InputField inputField;

    public enum ENotePadType
    {
        Default,
        Password,
    }
    [SerializeField]
    private ENotePadType notePadType;

    protected override void Init()
    {
        base.Init();
        inputField.onSelect.AddListener((s) => SelectWindow());

        OnSelected += inputField.ActivateInputField;
        OnUnSelected += () => inputField.DeactivateInputField();

    }



}
