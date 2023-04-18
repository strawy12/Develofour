using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiscordImagePanel : MonoBehaviour, IPointerClickHandler
{
    public void Init()
    {
        EventManager.StartListening(EDiscordEvent.ShowImagePanel, ReSetting);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Destroy(this.gameObject.transform.GetChild(0).gameObject);
            this.gameObject.SetActive(false);
        }
    }

    public void ReSetting(object[] obj)
    {
        //프리팹의 크기는 조절 못할듯
        //trigger의 크기가 고정되있어서
        //image viewer 구조부터 바꿔야지
        //가능할듯 싶음
        
        if(obj[0] is GameObject)
        {
            GameObject prefab = obj[0] as GameObject;
            GameObject instance = Instantiate(prefab, this.gameObject.transform);
            instance.GetComponent<ImageEnlargement>().Init(true);
            gameObject.SetActive(true);
        }

        return;
    }
}
