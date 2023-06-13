using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HomeProfile : MonoBehaviour, IPointerClickHandler
{
    public GameObject loginPanel;

    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private TMP_Text loginNameText;
    [SerializeField]
    private TMP_Text loginGamilText;

    [SerializeField]
    private string zmailString = "Heaven's Cloud@Gamil.com";

    public void Init()
    {
        loginButton.onClick.AddListener(ChangeLoginSite);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        loginPanel.gameObject.SetActive(true);

        if(DataManager.Inst.GetIsLogin(ELoginType.Zoogle))
        {
            ChangeLoginPanel();
        }
    }

    public void HidePanel()
    {
        loginPanel.gameObject.SetActive(false);
    }

    public void ChangeLoginSite()
    {
        EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { ESiteLink.GoogleLogin, Constant.LOADING_DELAY });
    }

    private void ChangeLoginPanel()
    {
        loginButton.gameObject.SetActive(false);

        loginNameText.SetText("강요한");
        loginGamilText.SetText(zmailString);
    }
}
