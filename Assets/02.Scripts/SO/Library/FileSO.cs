﻿using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[System.Serializable]
public struct AdditionFile
{
    public string fileID;
    public string directoryID;
}

[System.Serializable]
public struct WindowIconData
{
    public float bytes;
    public string madeDate;
    public string lastFixDate;
    public string lastAccessDate;
    public IconPropertyBody propertyBody;
}

[CreateAssetMenu(menuName = "SO/Library/fileSO")]
public class FileSO : ScriptableObject
{
    [SerializeField]
    private string id;
    public string ID
    {
        get => id;
        set
        {
            if (!string.IsNullOrEmpty(id))
                return;

            id = value;
        }
    }

    [HideInInspector]
    public string parentID; //SO 세팅에서 부모 세팅해줄때 사용할거
    public DirectorySO parent;
    public string fileName; // Data 불러주거나 같은 Window끼리 구분하는 키 값
    public EWindowType windowType; // 확장자 -> 매칭 시켜놓자 (WindowManager)
    public bool isHide;

    public Sprite iconSprite;
    public Color iconColor = Color.black;

    public WindowIconData propertyData;

    public bool isAlarm;


    #region GetFileData

#if UNITY_EDITOR
    public string GetRealFileLocation()
    {
        string location = "";
        Debug.Log($"{id}, {(parent  == null? "X" : parent.id)}");
        if (parent == null)
        {
            location = this.id + '\\';
            //Debug.Log($"{location}");
            return location;
        }

        location = string.Format("{0}{1}\\", parent.GetRealFileLocation(), this.id);
       //Debug.Log($"{location}");
        return location;
    }
#endif

    public string GetFileLocation()
    {
        string location = "";
        if (parent == null)
        {
            location = this.fileName + '\\';

            return location;
        } 

        location = string.Format("{0}{1}\\", parent.GetFileLocation(), this.fileName);

        return location;
    }

    public string GetFileLocationToSlash()
    {
        string location = "";
        if (parent == null)
        {
            location = this.fileName + '/';

            return location;
        }

        location = string.Format("{0}{1}/", parent.GetFileLocationToSlash(), this.fileName);

        return location;
    }

#if UNITY_EDITOR
    [ContextMenu("GetFileLocation")]
    public string DebugGetFileLocation()
    {
        string location = "";
        if (parent == null)
        {
            location = this.fileName + '\\';
            
            Debug.Log(location);
            return location;
        }

        location = string.Format("{0}{1}\\", parent.GetFileLocation(), this.fileName);

        Debug.Log(location);

        return location;
    }

#endif


    [ContextMenu("GetFileBytes")]
    public void Test()
    {
        Debug.Log(GetFileBytes());
    }

    public virtual float GetFileBytes()
    {
        return propertyData.bytes;
    }


    // DateTime To String 함수는 Define 클래스에 만들기!
    public string GetFixDate()
    {
        return propertyData.lastFixDate;
    }

    public string GetAccessDate()
    {
        return propertyData.lastAccessDate;
    }

    public string GetMadeDate()
    {
        return propertyData.madeDate;
    }
    #endregion
    public void OpenFile()
    {
        // AccessDate 시간을 변경해줄 예정
    }

    public void FixFile()
    {
        // FixDate 시간을 변경해줄 예정
    }

    private bool ReturnBool(string str)
    {
        if(str == "1" || str == "true" || str == "True" || str == "TRUE")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

// Window - Data - Icon - File
// Window -> 윈도우 창
// Data -> 윈도우Data (우리가 기존에 쓰던 Data X) -> 각 윈도우마다 필요한 데이터 모음집, 이미지뷰어면 이미지가 필요하겠지?
// Discord면 Chat기록이나 그런게 필요
// Icon -> Window 아이콘 
// File -> 기존에 원래 쓰던 WindowData

// TitleID를 썼던 이유는 다른 프리팹을 또 만들어야하잖아
// Window가 WIndowData를 들고있어야하는데 -> 프리팹 하나로 만들면 어떻게 불러줄거냐?
// Icon -> File (Icon은 그저 GUI임)
// Window -> File
// WIndow, Icon 둘다 File을 들고있으면 공통된 점으로 매치시켜서 실행