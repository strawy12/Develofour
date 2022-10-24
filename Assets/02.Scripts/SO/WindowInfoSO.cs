using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/UI/WindowInfo")]
public class WindowInfoSO : ScriptableObject
{
    [SerializeField] private string windowName;
    [SerializeField] private Vector2 pos;
    [SerializeField] private Vector2 size = new Vector2(1280, 720);

    public string WindowName => windowName;
    public Vector2 Pos => pos;
    public Vector2 Size => size;
}
