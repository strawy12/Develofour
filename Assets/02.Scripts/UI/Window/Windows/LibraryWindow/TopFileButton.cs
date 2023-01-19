using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopFileButton : MonoBehaviour
{
    private Button fileBtn;
    private TMP_Text fileName;

    private FileSO fileData;

    public void Init()
    {
        fileBtn.onClick.AddListener(OpenFIle);
    }

    private void OpenFIle()
    {
        object[] ps = new object[1] { fileData };
        EventManager.TriggerEvent(ELibraryEvent.OpenFile, ps);
    }
}
