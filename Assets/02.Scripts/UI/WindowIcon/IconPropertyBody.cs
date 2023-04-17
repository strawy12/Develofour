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


    public void Init(FileSO file)
    {
        iconImage.sprite = file.iconSprite;
        iconName.text = file.name;
        if(file.windowType != EWindowType.ImageViewer)
        {
            iconImage.color = Color.black;
        }
        string location = file.GetFileLocation();

        location = location.Replace('\\', '/');
        iconLocation.text = location;
        iconByte.text = file.GetFileBytes().ToString() + "KB";
        iconMadeData.text = file.GetMadeDate();
        iconFixData.text = file.GetFixDate();
        iconAccessData.text = file.GetAccessDate();
    }

}
