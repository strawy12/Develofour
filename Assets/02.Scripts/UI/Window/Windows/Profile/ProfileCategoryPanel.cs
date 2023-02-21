using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

public class ProfileCategoryPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;

    public EProfileCategory profileCategory;

    [SerializeField]
    private Image yellowUI;

    private ProfileInfoPanel infoPanel;

    [SerializeField]
    private Button InfoBtn;
    //info패널을 들고있음
    public void Init(EProfileCategory categoryEnum, string name, ProfileInfoPanel infoPanel)
    {
        profileCategory = categoryEnum;
        nameText.text = name;
        this.infoPanel = infoPanel;
        InfoBtn.onClick.AddListener(ShowInfoPanel);
    }

    public void ChangeValue(string key)
    {
        infoPanel.Setting(key);
    }
    private void ShowInfoPanel()
    {
        if(profileCategory == EProfileCategory.Owner)
        {
            EventManager.TriggerEvent(ETutorialEvent.ProfileInfoEnd);
        }
        infoPanel.gameObject.SetActive(true);
    }

    private bool isSign;
    public IEnumerator YellowSignCor()
    {
        yellowUI.gameObject.SetActive(true);
        isSign = true;
        while (isSign)
        {
            yellowUI.DOColor(new Color(255, 255, 255, 0.5f), 2f);
            yield return new WaitForSeconds(2f);
            yellowUI.DOColor(new Color(255, 255, 255, 1), 2f);
            yield return new WaitForSeconds(2f);
        }
    }

    public void StopCor()
    {
        isSign = false;
        yellowUI.gameObject.SetActive(false);
        StopCoroutine(YellowSignCor());
        yellowUI.DOKill();

        EventManager.StopListening(ETutorialEvent.ProfileInfoStart, delegate { StartCoroutine(YellowSignCor()); });
        EventManager.StopListening(ETutorialEvent.ProfileInfoEnd, delegate { StopCor(); });
    }

}
