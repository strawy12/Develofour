using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/Library/DirectorySO")]
public class DirectorySO : FileSO
{
    public List<FileSO> children;
}
