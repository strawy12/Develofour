using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;
using UnityEngine.Rendering.LookDev;
using UnityEditor.Compilation;

public class CreateSoundWindow : EditorWindow
{
    private ObjectField clipField;
    private TextField nameField;
    private Toggle isEffectToggle;

    [MenuItem("Tools/CreateSoundWindow")]
    public static void ShowWindow()
    {
        CreateSoundWindow win = GetWindow<CreateSoundWindow>();
        win.minSize = new Vector2(400, 250);
        win.maxSize = new Vector2(400, 250);
    }

    private void OnEnable()
    {
        VisualTreeAsset xml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/10.CustomWindows/Editor/CreateSoundWindow/CreateSoundWindow.uxml");

        TemplateContainer tree = xml.CloneTree();
        rootVisualElement.Add(tree);

        clipField = rootVisualElement.Q<ObjectField>("ClipField");
        nameField = rootVisualElement.Q<TextField>("NameField");
        isEffectToggle = rootVisualElement.Q<Toggle>("IsEffectToggle");

        Button btn = rootVisualElement.Q<Button>("CtreateBtn");
        btn.RegisterCallback<MouseUpEvent>(x => CreateAudioPlayer());
    }

    private void CreateAudioPlayer()
    {
        if (clipField.value == null)
        {
            Debug.LogError("ClipField's value is Null");
            return;
        }

        GameObject sound = GameObject.Find("Sound");

        if (sound == null)
        {
            Debug.LogError("Sound를 찾을 수 없습니다.");
            return;
        }

        const string PATH = "Assets/02.Scripts/Core/Sound/ESound.cs";

        if (System.IO.File.Exists(PATH) == false)
        {
            Debug.LogError($"ENoticeType 파일이 해당 경로에 존재하지 않습니다. 파일 위치를 옮겨주세요. {PATH}");
            return;
        }

        string valueText = nameField.value;

        if (string.IsNullOrEmpty(valueText))
        {
            valueText = clipField.value.name;


            while (!('A' <= valueText[0] && valueText[0] <= 'Z') &&
                   !('a' <= valueText[0] && valueText[0] <= 'z'))
            {
                Debug.Log(valueText);
                valueText = valueText.Substring(1);
            }
        }

        if ('a' <= valueText[0] || valueText[0] <= 'z')
        {

            char replaceChar = char.ToUpper(valueText[0]);
            valueText = replaceChar + valueText.Substring(1);
        }




        // Enum 
        // File.Write ,File.Read  cs 파일을 못읽어오고 못 써
        string text = "";
        using (StreamReader reader = new StreamReader(PATH))
        {
            text = reader.ReadToEnd();
            text.Replace("\r", "");
        }

        int length = text.Length - 1;

        int idx = isEffectToggle.value ? text.LastIndexOf("End") : text.IndexOf("End");
        text = text.Substring(0, idx) + $"{valueText},\n" + text.Substring(idx - 8);

        using (StreamWriter writer = new StreamWriter(PATH))
        {
            writer.Write(text);

            AssetDatabase.Refresh();
            CompilationPipeline.RequestScriptCompilation();
        }

        SoundPlayer soundPlayer = null;

        if (isEffectToggle.value)
        {
            soundPlayer = new GameObject(valueText).AddComponent<EffectPlayer>();
        }

        else
        {
            soundPlayer = new GameObject(valueText).AddComponent<BGMPlayer>();
        }


        soundPlayer.SetValue(clipField.value as AudioClip);

        soundPlayer.transform.SetParent(sound.transform.GetChild(isEffectToggle.value ? 1 : 0));
        soundPlayer.gameObject.SetActive(false);

        soundPlayer.AudioSourceInit();
    }
}
