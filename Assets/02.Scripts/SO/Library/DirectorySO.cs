using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(menuName = "SO/Library/DirectorySO")]
public class DirectorySO : FileSO
{
    public List<FileSO> children;

#if UNITY_EDITOR
    [ContextMenu("Debugs")]
    public void SS()
    {
        string[] guids = AssetDatabase.FindAssets("t:FileSO", null);
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            FileSO so = AssetDatabase.LoadAssetAtPath<FileSO>(path);
            Debug.Log($"ID: {so.id}");
            Debug.Log($"FileName: {so.fileName}");
            Debug.Log($"WindowType: {so.windowType}");

            if (so.isFileLock)
            {
                Debug.Log($"Pin: {so.windowPin}");
                Debug.Log($"Pin Hint: {so.windowPinHintGuide}");
            }

            if (so is DirectorySO)
            {
                Debug.Log(string.Join(", ", (so as DirectorySO).children));
            }
        }
    }
#endif

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
