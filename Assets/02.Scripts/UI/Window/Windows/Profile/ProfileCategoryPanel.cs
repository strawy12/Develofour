using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileCategoryPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;

    public EProfileCategory profileCategory;

    private ProfileInfoPanel infoPanel; 

    //info�г��� �������

    public void ChangeValue(string key)
    {
        infoPanel.Setting(key);
    }

}
