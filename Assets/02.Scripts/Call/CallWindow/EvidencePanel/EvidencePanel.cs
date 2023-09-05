using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceType
{
    public int maxCount; // 몇번 틀려야지 정답을 알려줄건가  0 == 안알려줌
    public string selectMonolog; // 제시했을때 나오는 독백
    public string wrongMonolog; // 틀렸을때 나오는 독백
    public string wrongHintMonolog; //틀렸고 Maxcount가 찼을떄 나오는 독백
}

public class EvidencePanel : MonoBehaviour
{
    public static EvidencePanel evidencePanel;

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
    private EvidenceType currentEvidenceType;

    [Header("디버그용")]
    [SerializeField]
    private ProfilerInfoText selectedInfoText;

    [SerializeField]
    private int wrongCnt;

    public void Awake()
    {
        evidencePanel = this;
        this.gameObject.SetActive(false);
        Debug.Log(evidencePanel);
    }

    public void Init(string _answerInfoID, EvidenceType evidenceType)
    {
        ReSetting();
        wrongCnt = 0;
        answerInfoID = _answerInfoID;
        currentEvidenceType = evidenceType;
        infoPanel.Init(true);
        typePanel.Init();
        characterBtn.onClick.AddListener(OnClickCharacterPanelBtn);
        sceneBtn.onClick.AddListener(OnClickIncidentPanelBtn);
        presentButton.onClick.AddListener(TryAnswer);
        InputManager.Inst.AddKeyInput(KeyCode.Space, onKeyDown: TryAnswer);

        this.gameObject.SetActive(true);
    }

    private void TryAnswer()
    {
        //인포 텍스트 가 answerID랑 같다면
        if(GetIsSelected())
        {
            MonologSystem.OnStartMonolog?.Invoke(currentEvidenceType.selectMonolog, false);

            if (selectedInfoText.InfoData.ID == answerInfoID)
            {
                Answer();
            }
            else
            {
                wrongCnt++;
                if(wrongCnt >= currentEvidenceType.maxCount)
                {
                    MonologSystem.AddOnEndMonologEvent(currentEvidenceType.selectMonolog, () =>
                    { MonologSystem.OnStartMonolog?.Invoke(currentEvidenceType.wrongHintMonolog, false); });
                }
                else
                {
                    MonologSystem.AddOnEndMonologEvent(currentEvidenceType.selectMonolog, () =>
                    { MonologSystem.OnStartMonolog?.Invoke(currentEvidenceType.wrongMonolog, false); });
                }
            }
        }
    }

    private bool GetIsSelected()
    {
        bool flag = false;

        bool one = false;
        foreach(var infoText in infoPanel.InfoTextList)
        {
            if (infoText.isSelected)
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
