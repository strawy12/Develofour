using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/Notepad/Data")]
public class NotepadDataSO : SOParent
{
    public string fileName;
    [TextArea]
    public string scripts;

    public override void Setting(string[] ps)
    {
        fileName = ps[1];
        string temp = ps[2];
        temp = temp.Replace("\\n", "\n");
        scripts = temp;
    }

}
