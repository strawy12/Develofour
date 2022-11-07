using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EWindowType
{
    None,
    Notepad,
    Browser,
}
[CreateAssetMenu(menuName = "SO/Window/Data")]
public class WindowDataSO : ScriptableObject
{
    public EWindowType windowType;

    [HideInInspector]
    public int windowTitleID;
    public Sprite iconSprite;
    [HideInInspector]
    
    public bool isMaximum;
    public Vector2 size;
    public Vector2 pos;
}
