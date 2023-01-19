using UnityEngine;

[System.Serializable]
public struct WindowIconData
{
    public string iconLocation;
    public string iconByte;
    public string iconMadeData;
    public string iconFixData;
    public string iconAcessData;

}

[CreateAssetMenu(menuName = "SO/Library/fileSO")]
public class FileSO : ScriptableObject
{
    public DirectorySO parent;

    public EWindowType windowType; // 확장자 -> 매칭 시켜놓자 (WindowManager)
    public Sprite iconSprite;
    //public string windowName; // Data 불러주거나 같은 Window끼리 구분하는 키 값
    public WindowIconData windowIconData;
    public bool isMultiple; // 윈도우를 여러번 킬 수 있냐
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