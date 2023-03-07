using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notepad : Window
{
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private CheckNotePadType checkNotePad;
    public NotepadDataSO currentData;

    [SerializeField]
    private float scrollValue = 7;

    protected override void Init()
    {
        base.Init();
        inputField.readOnly = true;
        OnSelected += inputField.ActivateInputField;
        OnUnSelected += () => inputField.DeactivateInputField();

        currentData = ResourceManager.Inst.GetNotepadData(file.name);

        inputField.scrollSensitivity = scrollValue;
        checkNotePad.Setting(file);

        if(currentData.name == "ZooglePassword" && !GuideManager.Inst.guidesDictionary[EGuideType.ClickPinNotePadHint])
        {
            GuideManager.Inst.isZooglePinNotePadOpenCheck = true;
        
            EventManager.TriggerEvent(ECoreEvent.OpenPlayGuide, new object[2] { 5f, EGuideType.ClickPinNotePadHint});
        }

        if (currentData.name == "ZooglePINword" && GuideManager.Inst.isZooglePinNotePadOpenCheck)
        {
            GuideManager.Inst.guidesDictionary[EGuideType.ClickPinNotePadHint] = true;

            EventManager.TriggerEvent(ECoreEvent.OpenPlayGuide, new object[2] { 5f, EGuideType.ClearPinNotePadQuiz });
        }

        SetText();
    }

    public void SetText()
    {
        if (file.windowName == "NotePad")
        {
            inputField.readOnly = false;
        }

        if (currentData == null)
        {
            return;
        }
        inputField.text = currentData.scripts;
    }


}
