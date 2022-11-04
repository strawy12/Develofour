using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TextBox : MonoUI
{
    private string getListText;

    [SerializeField]
    private TextMeshProUGUI boxShowText;

    [SerializeField]
    private TextDataSO textDataSO;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            NextPrint();
        }
    }

    private void Init()
    {
        GameManager.Inst.ChangeGameState(EGameState.UI);
    }

    public void ShowBox()
    {

    }

    public void HideBox()
    {
        SetActive(false);
    }

    public void PrintText()
    {

    }
    
    public void NextPrint()
    {

    }
}
