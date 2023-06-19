using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconPropertyBody : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private TMP_Text iconName;
    [SerializeField]
    private TMP_Text iconLocation;
    [SerializeField]
    private TMP_Text iconByte;
    [SerializeField]
    private TMP_Text iconMadeData;
    [SerializeField]
    private TMP_Text iconFixData;
    [SerializeField]
    private TMP_Text iconAccessData;
    [SerializeField]
    private Sprite pictureSprite;


    public void Init(FileSO file)
    {
        if(file.windowType == EWindowType.ImageViewer)
        {
            iconImage.sprite = pictureSprite;
            iconImage.color = Color.black;
            iconImage.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 45);
        }
        else
        {
            iconImage.sprite = file.iconSprite;
            iconImage.color = file.color;
        }

        iconName.text = file.name;
        string location = file.GetFileLocation();

        location = location.Replace('\\', '/');
        iconLocation.text = location;
        iconByte.text = file.GetFileBytes().ToString() + "KB";
        iconMadeData.text = file.GetMadeDate();
        iconFixData.text = file.GetFixDate();
        string accessDate = DataManager.Inst.GetLastAcccestDate(file.id);
        if (accessDate == "")
        {
            iconAccessData.text = file.GetAccessDate();
        }
        else
        {
            iconAccessData.text = accessDate;
        }
    }

}
