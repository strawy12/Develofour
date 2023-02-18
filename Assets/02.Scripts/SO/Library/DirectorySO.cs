using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/Library/DirectorySO")]
public class DirectorySO : FileSO
{
    public List<FileSO> children;

    public override int GetFileBytes()
    {
        int bytes = 0;

        foreach(FileSO child in children)
        {
            bytes += child.GetFileBytes();
        }

        return bytes;
    }

    public override void Setting(string[] str)
    {
        base.Setting(str);
        children.Clear();
    }
}
