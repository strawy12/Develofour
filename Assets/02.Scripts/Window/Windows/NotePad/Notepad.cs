using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notepad : Window
{
    [SerializeField]
    private NotepadBody notepadBody;

    private NotepadDataSO currentData;

    [SerializeField]
    private float scrollValue = 7;

    protected override void Init()
    {
        base.Init();
        Debug.Log("메모장 데이터 불러오는 키 값을 주소 값으로 변경해주세요");
        currentData = ResourceManager.Inst.GetNotepadData(file.GetFileLocation());

        if(currentData.notepadBody != null)
        {
            Transform parent = notepadBody.transform.parent;
            Destroy(notepadBody.gameObject);
            notepadBody = Instantiate(currentData.notepadBody, parent);
        }

        notepadBody.Init();

        notepadBody.inputField.readOnly = currentData.readOnly;

        OnSelected += notepadBody.inputField.ActivateInputField;
        OnUnSelected += () => notepadBody.inputField.DeactivateInputField();

        notepadBody.inputField.scrollSensitivity = scrollValue;

        EventManager.TriggerEvent(EGuideEventType.GuideConditionCheck, new object[] { file });

        SetText();
    }

    public void SetText()
    {
        if (currentData == null)
        {
            return;
        }
        notepadBody.inputField.text = currentData.scripts;
    }


}
