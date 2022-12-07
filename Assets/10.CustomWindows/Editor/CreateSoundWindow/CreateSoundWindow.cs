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

        Button btn = rootVisualElement.Q<Button>("CtreateBtn");
        btn.RegisterCallback<MouseUpEvent>(x => CreateAudioPlayer());
    }

    private void CreateAudioPlayer()
    {
        GameObject sound = GameObject.Find("Sound");

        if (sound == null)
        {
            Debug.LogError("Sound를 찾을 수 없습니다.");
            return;
        }

        const string PATH = "Assets/02.Scripts/Core/Sound/ESound.cs";

        if (File.Exists(PATH) == false)
        {
            Debug.LogError($"ENoticeType 파일이 해당 경로에 존재하지 않습니다. 파일 위치를 옮겨주세요. {PATH}");
            return;
        }

        string valueText = nameField.value;

        if ('a' <= nameField.value[0] || nameField.value[0] <= 'z')
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
        }

        int cnt = 0;
        int length = text.Length - 1;

        for (int i = length; i > 0; i--)
        {
            if (text[i] == '}')
            {
                if (cnt++ == 0)
                    continue;

                text = text.Insert(i - 11, $"{valueText},\n        ");
                break;
            }
        }

        Debug.Log(text);
      
        return;
        using (StreamWriter writer = new StreamWriter(PATH))
        {

            //writer.Write(text);
            //writer.Flush();

            //AssetDatabase.Refresh();
            //CompilationPipeline.RequestScriptCompilation();
        }



        EffectPlayer audioPlayer = new GameObject(nameField.text).AddComponent<EffectPlayer>();

        audioPlayer.Clip = clipField.value as AudioClip;

        audioPlayer.transform.SetParent(sound.transform);
        audioPlayer.gameObject.SetActive(false);

        audioPlayer.AudioSourceInit();
    }
}
