using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notepad : Window
{
    [SerializeField]
    private TMP_InputField inputField;

    public NotepadDataSO currentData;

    protected override void Init()
    {
        base.Init();
        inputField.onSelect.AddListener((s) => SelectWindow());

        OnSelected += inputField.ActivateInputField;
        OnUnSelected += () => inputField.DeactivateInputField();
        currentData = ResourceManager.Inst.GetNotepadData(file.name);

        SetText();
    }

    public void SetText()
    {
        if(currentData == null)
        {
            Debug.Log("Notepad Data∞° Null¿”");
            return;
        }
        inputField.text = currentData.scripts;
    }


}
