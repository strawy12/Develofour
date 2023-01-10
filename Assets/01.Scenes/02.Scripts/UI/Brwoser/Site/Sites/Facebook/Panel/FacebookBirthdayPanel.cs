using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacebookBirthdayPanel : MonoBehaviour
{
    [SerializeField]
    private List<FacebookFriendLine> facebookFriendLineList;

    //날짜 데이터를 가지고있는 싱글톤이 생긴다면 그것을 가져와야함;
    [SerializeField]
    private int currentMonth;
    [SerializeField]
    private int currentDay;

    public void Init(List<FacebookFriendDataSO> friendList)
    {
        int cnt = 0;
        for(int i = 0; i < friendList.Count; i++)
        {
            if(friendList[i].day == currentDay 
                && friendList[i].month == currentMonth)
            {
                if (cnt > facebookFriendLineList.Count) 
                    { Debug.LogError("배열 값 넘어 섬"); return; }
                facebookFriendLineList[cnt++].Setting(friendList[i]);
            }
        }
    }
}
