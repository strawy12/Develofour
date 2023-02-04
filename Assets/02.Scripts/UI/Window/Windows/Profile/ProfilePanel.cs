using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EProfileCategory
{
    Owner,
    
}

enum OwnerCategory
{
    
}


public class ProfilePanel : MonoBehaviour
{
    //?? 어떻게든 저장가능 , 동적으로 SO생성시키고 
    [SerializeField]
    private List<ProfileCategoryPanel> categoryProfileList = new List<ProfileCategoryPanel>();
    //[SerializeField]
    //private List<ProfileInfoPanel> 

}
