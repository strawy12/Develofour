using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologSystem : MonoBehaviour
{
    public static Action<ETextDataType, float, int> OnStartMonolog;
    public static Action OnEndMonologEvent;
    public static Action<ETextDataType, float, int> OnTutoMonolog;
    public static Action OnStopMonolog;
    [SerializeField]
    private TextBox textBox;

    private void Awake()
    {
        Debug.Log("Awake");
        OnStartMonolog += StartMonolog;
        OnTutoMonolog += TutoMonolog;
        OnStopMonolog += StopMonolog;
    }

    public void StartMonolog(ETextDataType textDataType, float delay, int cnt)
    {
        StartCoroutine(StartMonologCoroutine(textDataType,delay, cnt, false));
    }
    public void TutoMonolog(ETextDataType textDataType, float delay, int cnt)
    {
        StartCoroutine(StartMonologCoroutine(textDataType, delay, cnt, true));
    }
    private void StopMonolog()
    {
        StopAllCoroutines();
        textBox.StopAllCoroutines();
        textBox.SetActive(false);
        GameManager.Inst.ChangeGameState(EGameState.Game);
    }



    private IEnumerator StartMonologCoroutine(ETextDataType textDataType, float startDelay, int cnt, bool isTuto)
    {
        yield return new WaitForSeconds(startDelay);

        GameManager.Inst.ChangeGameState(EGameState.CutScene);
        textBox.Init(textDataType, TextBox.ETextBoxType.Simple);
        for (int i = 0; i < cnt; i++)
        {
            yield return new WaitForSeconds(0.1f);
            textBox.PrintText();
            yield return new WaitUntil(() => textBox.IsClick);
        }
        yield return new WaitForSeconds(0.1f);
        if (isTuto)
        {
            GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        }else
        {
            GameManager.Inst.ChangeGameState(EGameState.Game);
        }
        OnEndMonologEvent?.Invoke();
    }

}
