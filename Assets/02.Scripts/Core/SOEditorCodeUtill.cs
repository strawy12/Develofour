#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
public class SOEditorCodeUtill : MonoBehaviour
{
        Texture2D texture2D; 

    public static SOParent GetAssetFileLoadPath(string path)
    {
        SOParent so = AssetDatabase.LoadAssetAtPath(path, typeof(SOParent)) as SOParent;
        return so;
    }
    public static Sprite GetSpriteLoadPath(string path)
    {
        if(path[0] == '0')
        {
            return null;
        }

        byte[] bytes = File.ReadAllBytes(path);
        Sprite sprite = null;
        Texture2D bmp;
        bmp = new Texture2D(8, 8);
        bmp.LoadImage(bytes, false);

        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Rect tRect = new Rect(0, 0, bmp.width, bmp.height);


        bmp.LoadImage(bytes, false);
        sprite = Sprite.Create(bmp, tRect, pivot);
        return sprite;
    }

    public static void AddEnum(string PATH, string enumName)
    {
        string text = "";
        using (StreamReader sr = new StreamReader(PATH))
        {
            text = sr.ReadToEnd();
        }

        using (StreamWriter writer = new StreamWriter(PATH))
        {
            int length = text.Length - 1;
            // ";
            for (int i = 0; i < length; i++)
            {
                if (text[i] == '}')
                {
                    text = text.Insert(i - 6, $"\t{enumName},\n");
                    break;
                }
            }

            writer.Write(text);
            writer.Flush();

            AssetDatabase.Refresh();
            CompilationPipeline.RequestScriptCompilation();
        }
    }
}
#endif