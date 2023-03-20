using UnityEngine;
using System;
[System.Serializable]
public struct DateTime
{
    // 추후 변수 추가 예정
    public int year;
    public int month;
    public int day; 
    public DateTime(int _year, int _month, int _day)
    {
        year = _year;
        month = _month;
        day = _day;
    }
}

[System.Serializable]
public struct WindowIconData
{
    public int bytes;
    public string madeDate;
    public string lastFixDate;
    public string lastAccessDate;
    public IconPropertyBody propertyBody;
}

[CreateAssetMenu(menuName = "SO/Library/fileSO")]
public class FileSO : SOParent
{
    public DirectorySO parent;
    public string fileName; // Data 불러주거나 같은 Window끼리 구분하는 키 값
    public EWindowType windowType; // 확장자 -> 매칭 시켜놓자 (WindowManager)
    public Sprite iconSprite;
    public WindowIconData fileData;
    public bool isMultiple; // 윈도우를 여러번 킬 수 있냐
    public bool isFileLock;
    public string windowPin;
    public string windowPinHintGuide;
    public bool isAlarm;
    public object AssetDatabase { get; private set; }
    [SerializeField]
    private string tags;
    public TaskBarData taskBarData;
    #region GetFileData

    [ContextMenu("GetFileLocation")]
    public string GetFileLocation()
    {
        if(parent == null)
        {
            return this.fileName + '\\';
        } 

        string location = string.Format("{0}{1}\\", parent.GetFileLocation(), this.fileName);
        return location;
    }

    [ContextMenu("GetFileBytes")]
    public void Test()
    {
        Debug.Log(GetFileBytes());
    }

    public virtual int GetFileBytes()
    {
        return fileData.bytes;
    }


    // DateTime To String 함수는 Define 클래스에 만들기!
    public string GetFixDate()
    {
        return fileData.lastFixDate;
    }

    public string GetAccessDate()
    {
        return fileData.lastAccessDate;
    }

    public string GetMadeDate()
    {
        return fileData.madeDate;
    }
    #endregion

    public bool SearchTag(string text)
    {
        string[] tagArray;
        tagArray = tags.Split(',');

        foreach(string tag in tagArray)
        {
            if(tag.Contains(text))
            {
                return true;
            }
        }
        return false;
    }


    public void OpenFile()
    {
        // AccessDate 시간을 변경해줄 예정
    }

    public void FixFile()
    {
        // FixDate 시간을 변경해줄 예정
    }


    public override void Setting(string[] str)
    {
#if UNITY_EDITOR
        DirectorySO directory = SOEditorCodeUtill.GetAssetFileLoadPath(str[0]) as DirectorySO;
        try
        {
            windowType = (EWindowType)Enum.Parse(typeof(EWindowType), str[3]);
        }
        catch
        {
            string path = "D:/unityproject/Develofour/Assets\02.Scripts/UI/Window/Window.cs";

            SOEditorCodeUtill.AddEnum(path, str[3]);
            windowType = (EWindowType)Enum.Parse(typeof(EWindowType), str[3]);

        }
        fileName = str[4];
        fileData.bytes = int.Parse(str[5]);
        fileData.madeDate = str[6];
        fileData.lastFixDate = str[7];
        fileData.lastAccessDate = str[8];
        isMultiple = ReturnBool(str[9]);
       // isWindowLockClear = ReturnBool(str[10]);
        windowPin = str[11];
        windowPinHintGuide = str[12];
#endif
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