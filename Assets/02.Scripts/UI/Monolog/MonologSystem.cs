using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologSystem : MonoBehaviour
{
    public static Action<ETextDataType, float> OnStartMonolog;
    public static Action OnEndMonologEvent;
    [SerializeField]
    private TextBox textBox;

    private void Awake()
    {
        Debug.Log("Awake");
        OnStartMonolog += StartMonolog;
    }

    public void StartMonolog(ETextDataType textDataType,float delay)
    {
        Debug.Log("StartMonolog");
        StartCoroutine(StartMonologCoroutine(textDataType,delay));
    }

    private IEnumerator StartMonologCoroutine(ETextDataType textDataType, float delay)
    {
        yield return new WaitForSeconds(delay);
        textBox.Init(textDataType, TextBox.ETextBoxType.Simple);
        textBox.PrintText();
    }

}
