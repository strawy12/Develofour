using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProfileCategoryPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TMP_Text nameText;

    public EProfileCategory profileCategory;

    private ProfileInfoPanel infoPanel; 

    //info패널을 들고있음
    public void Init(EProfileCategory categoryEnum, string name, ProfileInfoPanel infoPanel)
    {
        profileCategory = categoryEnum;
        nameText.text = name;
        this.infoPanel = infoPanel;
    }

    public void ChangeValue(string key)
    {
        infoPanel.Setting(key);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        infoPanel.gameObject.SetActive(true);
    }
}
