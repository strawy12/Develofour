using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiscordImagePanel : MonoBehaviour, IPointerClickHandler
{
    public Image profileImage;

    public void Init()
    {
        EventManager.StartListening(EDiscordEvent.ShowImagePanel, ReSetting) ;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.gameObject.SetActive(false);
        }
    }

    private int sideCount = 3;
    private int sideX = 160;
    private int sideY = 90;

    public void ReSetting(object[] obj)
    {
        if(obj[0] is Sprite)
        {
            Sprite sprite = obj[0] as Sprite;
            profileImage.rectTransform.sizeDelta = new Vector2(sprite.rect.width - (sideX * sideCount), sprite.rect.height - (sideY * sideCount));
            profileImage.sprite = sprite;
            this.gameObject.SetActive(true);
        }
        return;
    }
}
