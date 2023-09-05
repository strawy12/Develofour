using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class ProfilerTitle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private Image image;

    private string titleText;


    public void OnPointerEnter(PointerEventData eventData)
    {
        //Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hide();
    }

    public void Setting(string titleName)
    {
        titleText = titleName;
    }
    public async void Show()
    {
        image.gameObject.SetActive(true);
        nameText.SetText(titleText);
        await Task.Delay(100);
        image.rectTransform.sizeDelta = nameText.rectTransform.sizeDelta;
    }

    public void Hide()
    {
        image.gameObject.SetActive(false);
    }


}
