using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CallTopPanel : MonoBehaviour
{
    [SerializeField]
    private Button autoCompleteBtn;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private Button nextBtn;
    [SerializeField]
    private Button beforeBtn;
    [SerializeField]
    private TMP_Text phoneNumberText;
    private List<CharacterInfoDataSO> characterDataList;

    private List<string> phoneNumberList;

    private int currentIdx = 0;
    public void Init()
    {
        phoneNumberList = new List<string>();
        nextBtn.onClick.AddListener(OnClickNextBtn);
        beforeBtn.onClick.AddListener(OnClickBeforeBtn);
        autoCompleteBtn.onClick.AddListener(OnClickAutoBtn);
        List<CharacterInfoDataSO> characterDataList = ResourceManager.Inst.GetCharacterDataSOList();
        EventManager.StartListening(ECallEvent.AddAutoCompleteCallBtn, AddNewAutoPhoneNumber);
        this.characterDataList = characterDataList;
        currentIdx = 0;
        foreach (var data in characterDataList)
        {
            if (DataManager.Inst.IsSavePhoneNumber(data.phoneNum))
            {
                phoneNumberList.Add(data.phoneNum);
            }
        }

        if(phoneNumberList.Count != 0 )
        {
            nameText.text = ResourceManager.Inst.GetCharacterDataSO(phoneNumberList[currentIdx]).characterName;

        }else
        {
            gameObject.SetActive(false);
        }
    }

    private void AddNewAutoPhoneNumber(object[] ps)
    {
        if (!(ps[0] is string))
        {
            return;
        }

        string number = ps[0] as string;
        DataManager.Inst.AddSavePhoneNumber(number);

        if (!phoneNumberList.Contains(number))
        {
            phoneNumberList.Add(number);
        }
        gameObject.SetActive(true);
        nameText.text = ResourceManager.Inst.GetCharacterDataSO(phoneNumberList[currentIdx]).characterName;
    }


    private void OnClickNextBtn()
    {
        if (phoneNumberList.Count == 0)
        {
            return;
        }

        if (currentIdx + 1 < phoneNumberList.Count)
        {
            currentIdx += 1;
            nameText.text = ResourceManager.Inst.GetCharacterDataSO(phoneNumberList[currentIdx]).characterName;
        }
    }

    private void OnClickBeforeBtn()
    {
        if (phoneNumberList.Count == 0)
        {
            return;
        }

        if (currentIdx - 1 >= 0)
        {
            currentIdx -= 1;
            nameText.text = ResourceManager.Inst.GetCharacterDataSO(phoneNumberList[currentIdx]).characterName;
        }
    }

    private void OnClickAutoBtn()
    {
        if (phoneNumberList.Count == 0)
        {
            return;
        }

        phoneNumberText.text = phoneNumberList[currentIdx];
    }





}
