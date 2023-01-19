using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Libraay/fileSO")]
public class FileSO : ScriptableObject
{
    public EWindowType windowType;
    public Sprite windowIcon;
    public int windowTitleID;
    public bool isMultiple;

    public string windowTitle;

    public string iconLocation;
    public string iconByte;
    public string iconMadeData;
    public string iconFixData;
    public string iconAcessData;

    public DirectorySO parent;
}
