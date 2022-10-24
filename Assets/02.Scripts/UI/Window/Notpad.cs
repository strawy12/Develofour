using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notpad : Window
{
    private TMP_InputField inputField;

    protected override void Bind()
    {
        base.Bind();
        inputField = transform.Find("InputField").GetComponent<TMP_InputField>();
    }

    public override void Init()
    {
        base.Init();
        OnTurnOn += () => inputField.interactable = true;
        OnTurnOff += () => inputField.interactable = false;
    }


}
