using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/Notepad/Data")]
public class NotepadDataSO : ScriptableObject
{
    public string fileName;
    [TextArea]
    public string scripts;
}
