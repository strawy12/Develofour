using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
public partial class SOSettingWindow : EditorWindow
{
    const string URL = @"https://docs.google.com/spreadsheets/d/1AYfKB3JgfR8zXXDccow0jJCuqa-CJJP9cBjdOlUIusw/export?format=tsv&range=2:1000&edit#gid={0}";

    private string gid;
    [MenuItem("Tools/SOSettingWindow")]
    public static void ShowWindow()
    {
        SOSettingWindow win = GetWindow<SOSettingWindow>();
        win.minSize = new Vector2(350, 250);
        win.maxSize = new Vector2(350, 250);
    }

    private void OnEnable()
    {
        
    }
}