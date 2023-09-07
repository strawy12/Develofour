using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvidencePanel : MonoBehaviour
{
    private static EvidencePanel inst;

    public static EvidencePanel Inst { get =>  inst;  }

    public ProfilerInfoPanel infoPanel;
    [SerializeField]
    private ProfilerInventoryPanel typePanel;
    [SerializeField]
    private Button sceneBtn;
    [SerializeField]
    private Button characterBtn;
    [SerializeField]
    private Button presentButton;

    private string answerInfoID;
    private EvidenceTypeSO currentEvidenceData;

    [SerializeField]
    private GameObject evidenceCoverPanel;

    private ProfilerInfoText selectedInfoText;

    private int wrongCnt;

    public void Awake()
    {
        inst = this;
        this.gameObject.SetActive(false);
        Debug.Log(Inst);
    }

    public void Init(string evidenceID)
    {
        ReSetting();
        wrongCnt = 0;
        EvidenceTypeSO evidenceData = ResourceManager.Inst.GetResource<EvidenceTypeSO>(evidenceID);
        answerInfoID = evidenceData.answerInfoID;
        currentEvidenceData = evidenceData;

        infoPanel.Init(true);
        typePanel.Init(true);
        characterBtn.onClick.AddListener(OnClickCharacterPanelBtn);
        sceneBtn.onClick.AddListener(OnClickIncidentPanelBtn);
        presentButton.onClick.AddListener(TryAnswer);
        InputManager.Inst.AddKeyInput(KeyCode.Space, onKeyDown: TryAnswer);

        this.evidenceCoverPanel.SetActive(true);
        this.gameObject.SetActive(true);
    }

    private void TryAnswer()
    {
        //인포 텍스트 가 answerID랑 같다면
        if (GetIsSelected())
        {
            if (selectedInfoText.InfoData.ID == answerInfoID)
            {
                Answer();
                return;
            }
            else
            {
                wrongCnt++;
                if (wrongCnt >= currentEvidenceData.maxCount)
                {
                    MonologSystem.AddOnEndMonologEvent(currentEvidenceData.selectMonolog, () =>
                    { MonologSystem.OnStartMonolog?.Invoke(currentEvidenceData.wrongHintMonolog, false); });
                }
                else
                {
                    MonologSystem.AddOnEndMonologEvent(currentEvidenceData.selectMonolog, () =>
                    { MonologSystem.OnStartMonolog?.Invoke(currentEvidenceData.wrongMonolog, false); });
                }
            }
            MonologSystem.OnStartMonolog?.Invoke(currentEvidenceData.selectMonolog, false);
        }
    }

    private bool GetIsSelected()
    {
        bool flag = false;

        bool one = false;
        foreach (var infoText in infoPanel.InfoTextList)
        {
            if (infoText.isChecked)
            {
                if (one) { Debug.LogError("선택되있는 인포텍스트가 두개에요"); continue; }
                selectedInfoText = infoText;
                one = true;
                flag = true;
            }
        }
        return flag;
    }

    private void OnClickCharacterPanelBtn()
    {
        if (typePanel.CheckCurrentType(EProfilerCategoryType.Character) == false)
            typePanel.ShowCharacterPanel();
    }

    private void OnClickIncidentPanelBtn()
    {
        if (typePanel.CheckCurrentType(EProfilerCategoryType.Info) == false)
            typePanel.ShowScenePanel();
    }

    private void Answer()
    {
        EventManager.TriggerEvent(EEvidencePanelEvent.Answer);
        InputManager.Inst.RemoveKeyInput(KeyCode.Space, onKeyDown: TryAnswer);

        this.evidenceCoverPanel.SetActive(false);
        this.gameObject.SetActive(false);
    }

    private void ReSetting()
    {
        characterBtn.onClick.RemoveAllListeners();
        sceneBtn.onClick.RemoveAllListeners();
        presentButton.onClick.RemoveAllListeners();
        InputManager.Inst.RemoveKeyInput(KeyCode.Space, onKeyDown: TryAnswer);
    }

}
