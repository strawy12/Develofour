using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateNoticeDataWindow : EditorWindow
{

    private bool isNameInputFieldFocus = false;
    TextField nameInputField = null;
    TextField headInputField = null;
    TextField bodyInputField = null;
    FloatField delayInputField = null;

    [MenuItem("Tools/CreateNoticeData")]
    public static void ShowWindow()
    {
        CreateNoticeDataWindow win = GetWindow<CreateNoticeDataWindow>();
        win.minSize = new Vector2(400, 450);
        win.maxSize = new Vector2(400, 450);
    }

    private void OnEnable()
    {
        VisualTreeAsset xml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/10.CustomWindows/Editor/CreateNoticeData/CreateNoticeDataWindow.uxml");

        TemplateContainer tree = xml.CloneTree(); 
        rootVisualElement.Add(tree);

        RegisterNameInputField();
        RegisterCreateBtn();
        //mapBtn.RegisterCallback<MouseUpEvent>(e => GenerateMap());
        //Button buildBtn = rootVisualElement.Q<Button>("build-player");
        //buildBtn.RegisterCallback<MouseUpEvent>(e => BuildPlayer());
    }

    private void Update()
    {
        if(isNameInputFieldFocus)
        {
            Input.imeCompositionMode = IMECompositionMode.Off;
        }
    }

    private void RegisterNameInputField()
    {
        nameInputField = rootVisualElement.Q<TextField>("NameInputField");
        nameInputField.RegisterCallback<FocusInEvent>((a) => {
            isNameInputFieldFocus = true;
        }
        );
        nameInputField.RegisterCallback<FocusOutEvent>((a) =>
        {
            isNameInputFieldFocus = false;
            Input.imeCompositionMode = IMECompositionMode.Auto;
        });
    }

    private void RegisterCreateBtn()
    {
        Button createBtn = rootVisualElement.Q<Button>("CreateBtn");
        createBtn.RegisterCallback<MouseUpEvent>((a) => CreateNoticeData());
    }

    private bool Exception()
    {
        if (string.IsNullOrEmpty(nameInputField.value)) return false;
        //if (string.IsNullOrEmpty(headInputField.value)) return false;
        //if (string.IsNullOrEmpty(bodyInputField.value)) return false;
        //if (delayInputField.value < 0f) return false;

        return true;
    }

    private void CreateNoticeData()
    {
        if (Exception() == false) return;

        const string SAVE_PATH = "Assets/02.Scripts/UI/Notice/";
        const string FILE_NAME = "ENoticeType.cs";

        const string PATH = SAVE_PATH + FILE_NAME;

        string defaultText = "public enum ENoticeType\n{\n\tNone = -1,\n }\n"; 
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        if (!File.Exists(PATH))
        {
            File.WriteAllText(PATH, defaultText);
        }

        using (StreamWriter writer = File.CreateText(PATH))
        {
            Debug.Log(PATH);
            string text = File.ReadAllText(PATH);
            string addValueText = $"\t{nameInputField.value}\n ";

            string newText = text;

            for(int i = 16; i < text.Length; i++)
            {
                if (text[i] == '}')
                {
                    newText.Insert(i - 1, addValueText);
                    break;
                }
            }

            writer.Write(newText);
        }
    }
}
