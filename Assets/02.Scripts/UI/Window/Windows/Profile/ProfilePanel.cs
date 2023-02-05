using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EProfileCategory
{
    Owner,
    Boyfriend,
}

public enum OwnerCategory
{
    Email,
    Name
}
public enum BoyfriendCategory
{
    Age,
    Birth
}

public class ProfilePanel : MonoBehaviour
{
    //?? 어떻게든 저장가능 , 동적으로 SO생성시키고 
    [SerializeField]
    private List<ProfileCategoryPanel> categoryProfileList = new List<ProfileCategoryPanel>();


    //이벤트 매니저 등록
    private void ChangeValue(object[] ps) // 0 = 카테고리, 1 = key값 스트링, 
    {
        if(!(ps[0] is EProfileCategory) || !(ps[1] is string))
        {
            return;
        }

        foreach (var temp in categoryProfileList)
        {
            if(temp.profileCategory == (EProfileCategory)ps[0])
            {
                temp.ChangeValue(ps[1] as string);
            }
        }
    }
    //카테고리를 들고있음
}
