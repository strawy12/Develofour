using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/Notepad/Data")]
public class NotepadDataSO : ResourceSO
{
    //id is fileID
    public bool readOnly = true;

    public NotepadBody notepadBody;

    [TextArea]
    public string scripts;

    [ContextMenu("SetScripts")]
    public void SetScripts()
    {
        if(notepadBody != null)
        {
            scripts = notepadBody.GetComponent<TMPro.TMP_InputField>().text;
        }
    }
}
