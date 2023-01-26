using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacebookBirthdayPanel : MonoBehaviour
{
    [SerializeField]
    private List<FacebookFriendLine> facebookFriendLineList;

    //��¥ �����͸� �������ִ� �̱����� ����ٸ� �װ��� �����;���;
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
                    { Debug.LogError("�迭 �� �Ѿ� ��"); return; }
                facebookFriendLineList[cnt++].Setting(friendList[i]);
            }
        }
    }
}
