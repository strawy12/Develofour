using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/Alteration")]
public class WindowAlterationSO : SOParent
{
    public bool isMaximum;
    public Vector2 size;
    public Vector2 pos;
    public Vector2 saveSize;

    public override void Setting(string[] ps)
    {
        size.x = int.Parse(ps[1]);
        size.y = int.Parse(ps[2]);
        pos.x = int.Parse(ps[3]);
        pos.y = int.Parse(ps[4]);
        saveSize = size;
    }

    public WindowAlterationSO(WindowAlterationSO origin)
    {
        isMaximum = origin.isMaximum;
        size = origin.size;
        pos = origin.pos;
        saveSize = origin.saveSize;
    }
}
