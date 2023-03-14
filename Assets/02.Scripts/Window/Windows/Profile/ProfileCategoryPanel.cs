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

    [SerializeField]
    private Button InfoBtn;

    //info패널을 들고있음
    public void Init(EProfileCategory categoryEnum, string name, ProfileInfoPanel infoPanel)
    {
        profileCategory = categoryEnum;
        nameText.text = name;
        InfoBtn.onClick.AddListener(ShowInfoPanel);
    }


    private void ShowInfoPanel()
    {
        if(profileCategory == EProfileCategory.Owner)
        {
            if(!GameManager.Inst.profilerTutorialClear)
            {
                GameManager.Inst.profilerTutorialClear = true;
                EventManager.TriggerEvent(ETutorialEvent.ProfileInfoEnd, new object[1] { this } );
            }

        }

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
        if (!isSign) return;

        isSign = false;

        yellowUI.DOKill();
        yellowUI.gameObject.SetActive(false);
        StopAllCoroutines();

        EventManager.StopListening(ETutorialEvent.ProfileInfoStart, delegate { StartCoroutine(YellowSignCor()); });
        EventManager.TriggerEvent(ETutorialEvent.ProfileEventStop, new object[0]);
    }

}
