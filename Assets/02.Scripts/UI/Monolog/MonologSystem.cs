using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologSystem : MonoBehaviour
{
    public static Action<ETextDataType, float, int> OnStartMonolog;
    public static Action OnEndMonologEvent;
    [SerializeField]
    private TextBox textBox;

    private void Awake()
    {
        Debug.Log("Awake");
        OnStartMonolog += StartMonolog;
    }

    public void StartMonolog(ETextDataType textDataType, float delay, int cnt)
    {
        Debug.Log("StartMonolog");
        StartCoroutine(StartMonologCoroutine(textDataType,delay, cnt));
    }

    private IEnumerator StartMonologCoroutine(ETextDataType textDataType, float delay, int cnt)
    {
        GameManager.Inst.ChangeGameState(EGameState.CutScene);
        textBox.Init(textDataType, TextBox.ETextBoxType.Simple);
        for (int i = 0; i < cnt; i++)
        {
            yield return new WaitForSeconds(delay);
            textBox.PrintText();
            yield return new WaitUntil(() => textBox.IsClick);
        }
        GameManager.Inst.ChangeGameState(EGameState.Game);
    }

}
