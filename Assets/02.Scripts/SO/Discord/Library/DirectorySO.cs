using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Library/DirectorySO")]
public class DirectorySO : FileSO
{
    public List<FileSO> children;


    public override float GetFileBytes()
    {

        float bytes = 0;

        foreach (FileSO child in children)
        {
            bytes += child.GetFileBytes();
        }

        return bytes;
    }

    public override void Setting(string[] str)
    {
        base.Setting(str);
        if (children != null)
            children.Clear();
    }
}
