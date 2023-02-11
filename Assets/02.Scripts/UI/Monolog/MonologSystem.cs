using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologSystem : MonoBehaviour
{
    public static Action<ETextDataType> OnStartMonolog;
    public static Action OnEndMonologEvent;
    [SerializeField]
    private TextBox textBox;

    private void Awake()
    {
        Debug.Log("Awake");
        OnStartMonolog += StartMonolog;
    }

    public void StartMonolog(ETextDataType textDataType)
    {
        Debug.Log("StartMonolog");
        textBox.Init(textDataType, TextBox.ETextBoxType.Simple);
        textBox.PrintText();
    }

}
