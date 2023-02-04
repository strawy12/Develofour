using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileInfoPanel : MonoBehaviour
{ 
    public EProfileCategory category;

    //동적 저장을 위해서는 활성화 비활성화 여부를 들고있는 SO 혹은 Json이 저장 정보를 불러오고 저장





#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        //SO 초기화
    }
#endif
}