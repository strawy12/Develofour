using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Compilation;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
public class CreateNewSaveDataTool : EditorWindow
{
    [MenuItem("Tools/CreateNewSaveData")]
    public static void DeleteSaveData()
    {
        File.Delete(Application.persistentDataPath + "/Save/" + "Data.Json");
    }
}
