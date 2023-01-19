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
            //icon들을 children의 fileSO기반으로 설정
            //iconPrefab 
        }
    }

   



    //directory안에 있는 children들을 생성해줘야함, 여기서 windowIcon으로 생성해줘야함.
    //directory안의 directory를 들어가서 
}
