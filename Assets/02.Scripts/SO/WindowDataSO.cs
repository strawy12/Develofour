using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/UI/WindowData")]
public class WindowDataSO : ScriptableObject
{
    [SerializeField] private string windowName;
    [SerializeField] private string title;
    [SerializeField] private Vector2 pos;
    [SerializeField] private Vector2 size = new Vector2(1280, 720);
    [SerializeField] private Sprite iconSprite;

    public string WindowName => windowName;
    public string Title => title;
    public Sprite IconSprite => iconSprite;
    public Vector2 Pos => pos;
    public Vector2 Size => size;
}
