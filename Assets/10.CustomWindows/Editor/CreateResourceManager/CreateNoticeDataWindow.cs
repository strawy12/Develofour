using DG.Tweening.Plugins.Core.PathCore;
using NUnit.Framework.Internal.Execution;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateResourceManagerWindow : EditorWindow
{
    TextField nameInputField = null;
    TextField keyInputField = null;
    TextField labelInputField = null;
    TextField typeInputField = null;

    [MenuItem("Tools/CreateResourceManager")] 
    public static void ShowWindow()
    {
        CreateResourceManagerWindow win = GetWindow<CreateResourceManagerWindow>();
        win.minSize = new Vector2(390, 150);
        win.maxSize = new Vector2(390, 150);
    }

    private void OnEnable()
    {
        VisualTreeAsset xml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/10.CustomWindows/Editor/CreateResourceManager/CreateResourceManager.uxml");

        TemplateContainer tree = xml.CloneTree();
        rootVisualElement.Add(tree);

        BindInputField();

        RegisterCreateBtn();
    }

    private void BindInputField()
    {
        keyInputField = rootVisualElement.Q<TextField>("KeyField");
        labelInputField = rootVisualElement.Q<TextField>("LabelField");
        nameInputField = rootVisualElement.Q<TextField>("NameField");
        typeInputField = rootVisualElement.Q<TextField>("TypeField");
    }

    private void CreateScript()
    {
        // {0} == label
        // {1} == Type
        // {2} == Key
        // {3} == Name
        // {4} == 소문자 name

        string script = @"using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{{
    [SerializeField]
    private Dictionary<{2}, {1}> {4}List;

    public {1} Get{3}({2} key)
    {{
        return {4}List[key];
    }}

    private async void Load{3}Assets(Action callBack)
    {{
        {4}List = new Dictionary<{2}, {1}>();

        var handle = Addressables.LoadResourceLocationsAsync(""{0}"", typeof({1}));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {{
            var task = Addressables.LoadAssetAsync<{1}>(handle.Result[i]).Task;
            await task;

            {4}List.Add(key, task.Result);
        }}

        Addressables.Release(handle);

        callBack?.Invoke();
    }}
}}
";
        string lowerName = char.ToLower(nameInputField.text[0]) + nameInputField.text.Substring(1);
        string handleText = string.Format(script, labelInputField.text, typeInputField.text, keyInputField.text, nameInputField.text, lowerName);
        File.WriteAllText($"./Assets/02.Scripts/Core/Managers/ResourceManager/{nameInputField.text}Resources.cs", handleText);

        AssetDatabase.Refresh();

        Debug.Log($"성공적으로 {nameInputField.text}Resources.cs을 생성하였습니다.");
    }



    private void RegisterCreateBtn()
    {
        Button createBtn = rootVisualElement.Q<Button>("CreateBtn");
        createBtn.RegisterCallback<MouseUpEvent>((a) => CreateScript());
    }

}
