using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notepad : Window
{
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private CheckNotePadTyoe checkNotePad;
    public NotepadDataSO currentData;

    [SerializeField]
    private float scrollValue = 7;

    protected override void Init()
    {
        base.Init();

        OnSelected += inputField.ActivateInputField;
        OnUnSelected += () => inputField.DeactivateInputField();
        currentData = ResourceManager.Inst.GetNotepadData(file.name);
        inputField.scrollSensitivity = scrollValue;
        checkNotePad.Setting(file);
        SetText();
    }

    public void SetText()
    {
        if(currentData == null)
        {
            Debug.Log("Notepad Data가 Null임");
            return;
        }
        inputField.text = currentData.scripts;
    }


}
