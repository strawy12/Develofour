using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : Window
{
    [SerializeField]
    private DirectorySO currentDirectory;

    [SerializeField]
    private WindowIcon iconPrefab;



    protected override void Init()
    {
        base.Init();
        currentDirectory = fileSO as DirectorySO;
    }

    private void SettingChildren()
    {

        foreach (FileSO file in currentDirectory.children)
        {
            //icon���� children�� fileSO������� ����
            //iconPrefab 
        }
    }

   



    //directory�ȿ� �ִ� children���� �����������, ���⼭ windowIcon���� �����������.
    //directory���� directory�� ���� 
}
