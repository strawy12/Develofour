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

    public ProfileOverlayOpenTrigger overlayTrigger;

    protected override void Init()
    {
        base.Init();
 
        currentData = ResourceManager.Inst.GetNotepadData(file.id);

        bool useDataBody = currentData.notepadBody != null;
        if (useDataBody)
        {
            Transform parent = notepadBody.transform.parent;
            Destroy(notepadBody.gameObject);
            notepadBody = Instantiate(currentData.notepadBody, parent);
        }

        notepadBody.Init();

        notepadBody.inputField.readOnly = currentData.readOnly;

        OnSelected += notepadBody.inputField.ActivateInputField;
        OnUnSelected += () => notepadBody.inputField.DeactivateInputField();

        OnSelected += OverlayOpen;
        OnUnSelected += OverlayClose;
        OverlayOpen();

        windowBar.OnMaximum.AddListener(notepadBody.SetTriggerPosition);

        notepadBody.inputField.scrollSensitivity = scrollValue;

        EventManager.TriggerEvent(EGuideEventType.GuideConditionCheck, new object[] { file });

        SetText();
    }

    public override void WindowOpen(bool isNewOpen)
    {
        base.WindowOpen(isNewOpen);
        notepadBody.inputField.textComponent.ForceMeshUpdate();
        notepadBody.SetTriggerPosition();
    }

    public void SetText()
    {
        if (currentData == null)
        {
            return;
        }
        notepadBody.inputField.text = currentData.scripts;
    }

    private void OverlayClose()
    {
        if (overlayTrigger == null) // 없다면 찾아와
        {
            overlayTrigger = notepadBody.GetComponent<ProfileOverlayOpenTrigger>();
            if (overlayTrigger == null) { return; }
        }
        overlayTrigger.Close();
    }

    private void OverlayOpen()
    {
        if (overlayTrigger == null) // 없다면 찾아와
        {
            overlayTrigger = notepadBody.GetComponent<ProfileOverlayOpenTrigger>();
            if (overlayTrigger == null) { return; }
        }
        overlayTrigger.Open();
    }

    protected override void OnDestroyWindow()
    {
        base.OnDestroyWindow();
        OnSelected -= OverlayOpen;
        OnUnSelected -= OverlayClose;
    }
}
