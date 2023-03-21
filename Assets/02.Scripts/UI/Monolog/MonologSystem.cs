using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologSystem : MonoBehaviour
{
    public static Action<EMonologTextDataType, float, int> OnStartMonolog;
    public static Action OnEndMonologEvent;
    public static Action<EMonologTextDataType, float, int> OnTutoMonolog;
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

    public void StartMonolog(EMonologTextDataType textDataType, float delay, int cnt)
    {
        if (DataManager.Inst.IsMonologShow(textDataType))
        {
            OnEndMonologEvent = null;
            return;
        }
        StartCoroutine(StartMonologCoroutine(textDataType, delay, cnt, false));
    }
    public void TutoMonolog(EMonologTextDataType textDataType, float delay, int cnt)
    {
        if (DataManager.Inst.IsMonologShow(textDataType))
        {
            OnEndMonologEvent = null;
            return;
        }
        StartCoroutine(StartMonologCoroutine(textDataType, delay, cnt, true));
    }
    private void StopMonolog()
    {
        OnEndMonologEvent?.Invoke();
        StopAllCoroutines();
        textBox.StopAllCoroutines();
        textBox.SetActive(false);
        GameManager.Inst.ChangeGameState(EGameState.Game);
    }

    private IEnumerator StartMonologCoroutine(EMonologTextDataType textDataType, float startDelay, int cnt, bool isTuto)
    {
        yield return new WaitForSeconds(startDelay);

        GameManager.Inst.ChangeGameState(EGameState.CutScene);
        textBox.Init(textDataType);

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
        }
        else
        {
            GameManager.Inst.ChangeGameState(EGameState.Game);
        }

        OnEndMonologEvent?.Invoke();

        DataManager.Inst.SetMonologShow(textDataType, true);
    }

}
