using NUnit.Framework.Internal.Execution;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
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
        if (isNameInputFieldFocus)
        {
            Input.imeCompositionMode = IMECompositionMode.Off;
        }
    }

    private void RegisterNameInputField()
    {
        nameInputField = rootVisualElement.Q<TextField>("NameInputField");
        nameInputField.RegisterCallback<FocusInEvent>((a) =>
        {
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

    //private string Validation(string text)
    //{
    //    string newText = "";
    //    for(int i = 0; i < text.Length; i++)
    //    {
    //        if('a' < )
    //    }
    //}

    private void CreateNoticeData()
    {
        if (Exception() == false) return;

        const string PATH = "Assets/02.Scripts/UI/Notice/ENoticeType.cs";

        #region Bug Exception
        if (File.Exists(PATH) == false)
        {
            Debug.LogError($"ENoticeType 파일이 해당 경로에 존재하지 않습니다. 파일 위치를 옮겨주세요. {PATH}");
            return;
        }

        string valueText = nameInputField.value;

        if ('a' <= nameInputField.value[0] || nameInputField.value[0] <= 'z')
        {
            char replaceChar = char.ToUpper(valueText[0]);
            valueText = valueText.Replace(valueText[0], replaceChar);
            Debug.Log(valueText);
        }
        //if (Enum.IsDefined(typeof(ENoticeType), nameInputField.value))
        //{
        //    Debug.LogError($"ENoticeType에는 해당 Name이 이미 존재합니다. 이름을 변경해주세요. {nameInputField.value}");
        //    return;
        //}
        ////if (nameInputField.value)
        ////{
        ////    Debug.LogError($"Name은 숫자로 시작할 수 없습니다. 이름을 변경해주세요. {nameInputField.value}");
        ////    return;
        ////}
        #endregion

        try
        {
            Stream rs = new FileStream(PATH, FileMode.Open);
            rs.Close();
            
            using (StreamReader reader = new StreamReader(PATH))
            using (StreamWriter writer = new StreamWriter(PATH, false))
            {
                string text = reader.ReadToEnd();

                //if (string.IsNullOrEmpty(text))
                //{
                //    text = "public enum ENoticeType\n{\n\tNone = -1,\n }\n";
                //}


                string addValueText = $"\t{valueText}\n ";

                string newText = text;

                for (int i = 16; i < text.Length; i++)
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

        catch (IOException ie)
        {
            Debug.LogError("ENoticeType이 켜져있는 것 같습니다. ENoticeType 파일을 꺼주고 다시 진행해주세요");
            Debug.LogError(ie);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
