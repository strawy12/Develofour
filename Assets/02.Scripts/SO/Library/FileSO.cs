using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Libraay/fileSO")]
public class FileSO : ScriptableObject
{
    public EWindowType windowType;
    public Sprite windowIcon;
    public int windowTilteID;
    public bool isMultiple;

    public DirectorySO parent;
}
