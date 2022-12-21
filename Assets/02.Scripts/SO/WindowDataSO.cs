using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EWindowType
{
    None,
    Notepad,
    Browser,
    ImageViewer,
    End
}
[CreateAssetMenu(menuName = "SO/Window/Data")]
public class WindowDataSO : ScriptableObject
{
    public EWindowType windowType;

    public Sprite iconSprite;

    public int windowTitleID;

    [HideInInspector]
    public bool isMaximum;
    public Vector2 size;
    public Vector2 pos;
}
