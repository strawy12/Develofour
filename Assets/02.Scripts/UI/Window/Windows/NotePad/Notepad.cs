using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notepad : Window
{
    [SerializeField]
    private TMP_InputField inputField;

    public NotepadDataSO data;

    protected override void Init()
    {
        base.Init();
        inputField.onSelect.AddListener((s) => SelectWindow());

        OnSelected += inputField.ActivateInputField;
        OnUnSelected += () => inputField.DeactivateInputField();
        data = ResourceManager.Inst.GetNotepadData(file.name);

        SetText();
    }

    public void SetText()
    {
        if(data == null)
        {
            Debug.Log("Notepad Data∞° Null¿”");
            return;
        }
        inputField.text = data.scripts;
    }


}
