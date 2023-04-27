using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/Notepad/Data")]
public class NotepadDataSO : SOParent
{
    public string fileName;
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

    public override void Setting(string[] ps)
    {
        fileName = ps[1];
        string temp = ps[2];
        temp = temp.Replace("\\n", "\n");
        scripts = temp;
    }

}
