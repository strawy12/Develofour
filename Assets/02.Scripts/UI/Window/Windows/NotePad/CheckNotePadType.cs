using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckNotePadType : MonoBehaviour
{
    private FileSO currentFile;

    private void Awake()
    {

    }
    public void Setting(FileSO file)
    {
        currentFile = file;
    }
}
