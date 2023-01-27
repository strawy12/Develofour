using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/Alteration")]
public class WindowAlterationSO : ScriptableObject 
{
    [HideInInspector]
    public bool isMaximum;
    public Vector2 size;
    public Vector2 pos;
}
