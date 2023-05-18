using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/Alteration")]
public class WindowAlterationSO : ScriptableObject
{
    public bool isMaximum;
    public Vector2 size;
    public Vector2 pos;
    public Vector2 saveSize;

    public WindowAlterationSO(WindowAlterationSO origin)
    {
        isMaximum = origin.isMaximum;
        size = origin.size;
        pos = origin.pos;
        saveSize = origin.saveSize;
    }
}
