using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MonologSystem : TextSystem
{
    private void ProfilerFileException(object[] ps)
    {
        if(ps[0] is DirectorySO) {
            DirectorySO directory = ps[0] as DirectorySO;
            OpenDirectoryEx(directory);
        }
    }


    private void OpenDirectoryEx(DirectorySO directory)
    {
        
    }
}


