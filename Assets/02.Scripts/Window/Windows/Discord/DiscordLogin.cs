using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class DiscordLogin : MonoBehaviour
{
    [Header("기본 로그인 정답")]
    [SerializeField]
    private string answerID;
    [SerializeField]
    private string answerPassword;

    [Header("본인 확인 로그인 정답")]
    public string identificationAnswerText;

    [SerializeField]
    private TMP_InputField IDInputField;

    [SerializeField]
    private TMP_InputField passwordInputField;

    [SerializeField]
    private Button loginButton;

    [Header("오답텍스트")]
    [SerializeField]
    private TextMeshProUGUI wrongIDInputFieldText;
    [SerializeField]
    private TextMeshProUGUI wrongPasswordInputFieldText;

    [Header("기본텍스트")]
    [SerializeField]
    private TextMeshProUGUI currentIdInputFieldText;
    [SerializeField]
    private TextMeshProUGUI currentPasswordInputFieldText;

    //public DiscordAccountPanel IDAccountPanel;
    //public DiscordAccountPanel pwAccountPanel;

    public DiscordLoginBackground background;

    public DiscordIdentification identificationPanel;
    public GameObject loginPanel;

    public GameObject IDAccountPanel;
    public GameObject pwAccountPanel;

    private bool isLogin;

    public void Init()
    {
        background.OnIDPWPanelOff += SetIDPWPanel;
        identificationPanel.Init(identificationAnswerText);
        //IDAccountPanel.Init();
        //pwAccountPanel.Init();
        //IDAccountPanel.OnClick += SetIDText;
        //pwAccountPanel.OnClick += SetPWText;
        //IDInputField.OnShowAccount += ShowIDAccountPanel;
        //passwordInputField.OnShowAccount += ShowPWAccountPanel;
        loginButton.onClick.AddListener(OnClickLogin);
        Debug.Log("온클릭 시작");
        InputManager.Inst.AddKeyInput(KeyCode.Return, onKeyDown: OnClickLogin);
    }

    public void SetIDPWPanel()
    {
        if(!isLogin)
        {
            //IDAccountPanel.gameObject.SetActive(false);
            //pwAccountPanel.gameObject.SetActive(false);
        }    
    }

    public void ShowIDAccountPanel()
    {
        if(DataManager.Inst.IsMonologShow("T_CA_P_R_3"))
        {
            IDAccountPanel.gameObject.SetActive(true);
        }
    }

    public void ShowPWAccountPanel()
    {
        if(DataManager.Inst.IsProfilerInfoData("T_M_63"))
        {
            pwAccountPanel.gameObject.SetActive(true);
        }
    }

    public void OnClickLogin()
    {
#if UNITY_EDITOR

            SuccessLogin();
        
#endif
        //현재 내 윈도우가 가장 위에있는지 확인
        if (WindowManager.Inst != null && !WindowManager.Inst.IsTopWindow(EWindowType.Discord))
        {
            return;
        }

        if(IDInputField.text == answerID && passwordInputField.text == answerPassword)
        {
            //Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginSuccess);
            SuccessLogin();
        }
        else
        {
           // Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginFailed);
            if(IDInputField.text != answerID)
            {
                currentIdInputFieldText.gameObject.SetActive(false);
                wrongIDInputFieldText.gameObject.SetActive(true);
                wrongIDInputFieldText.text = "<b>이메일 또는 전화번호 </b>- <i><size=85%> 유효하지 않은 아이디입니다.</i>";
                //wrongIDInputFieldText.GetComponent<TextMove>().FaliedInput("<b>이메일 또는 전화번호 </b>- <i><size=85%> 유효하지 않은 아이디입니다.</i>");
            }

            if(passwordInputField.text != answerPassword)
            {
                currentPasswordInputFieldText.gameObject.SetActive(false);
                wrongPasswordInputFieldText.gameObject.SetActive(true);
                wrongPasswordInputFieldText.text = "<b>비밀번호 </b>- <i><size=85%> 유효하지 않은 비밀번호입니다.</i>";
                //wrongPasswordInputFieldText.GetComponent<TextMove>().FaliedInput("<b>비밀번호 </b>- <i><size=85%> 유효하지 않은 비밀번호입니다.</i>"); 
            }

        }
    }

    public void SuccessLogin()
    {
        //성공을 알리는 이벤트
        if (DataManager.Inst.IsClearTutorial() && !DataManager.Inst.IsProfilerInfoData(Constant.ProfilerInfoKey.KANGYOHAN_PHONENUMBER)) //id
        {
            //EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[] { EProfilerCategory.InvisibleInformation, Constant.ProfilerInfoKey.KANGYOHAN_PHONENUMBER });
        }

        if (DataManager.Inst.IsClearTutorial() && !DataManager.Inst.IsProfilerInfoData(Constant.ProfilerInfoKey.HARMONY_PASSWORD)) //password
        {
           // EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[] { EProfilerCategory.InvisibleInformation, Constant.ProfilerInfoKey.HARMONY_PASSWORD });
        }

        InputManager.Inst.RemoveKeyInput(KeyCode.Return, onKeyDown: OnClickLogin);

        isLogin = true;
        identificationPanel.gameObject.SetActive(true);
        loginPanel.SetActive(false);
    }

    void OnDisable()
    {
        if(InputManager.Inst != null)
        InputManager.Inst.RemoveKeyInput(KeyCode.Return, onKeyDown: OnClickLogin);
    }
}
