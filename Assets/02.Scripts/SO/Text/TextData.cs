using UnityEngine;

[System.Serializable]
public class TextData
{
    public string name;
    public Color color;

    [TextArea(5, 30)]
    public string text;
}

