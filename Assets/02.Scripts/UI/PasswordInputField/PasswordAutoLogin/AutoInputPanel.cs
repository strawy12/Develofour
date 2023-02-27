using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class AutoInputPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TMP_Text idText;

    [SerializeField]
    private TMP_Text passwordText;

    private TMP_InputField idInputFiled;

    private TMP_InputField passwordInputField;

    private bool isHaveID;

    [SerializeField]
    private AutoInputSystem inputSystem;

    public void Setting(List<TMP_InputField> inputFields, AutoAnswerData data)
    {
        if(inputFields.Count < 2)
        {
            passwordInputField = inputFields[0];
            passwordText.text = data.password;

            idText.gameObject.SetActive(false);
            isHaveID = false;
        }
        else
        {
            idInputFiled = inputFields[0];
            passwordInputField = inputFields[1];

            idText.text = data.id;
            passwordText.text = data.password;

            idText.gameObject.SetActive(true);
            isHaveID = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isHaveID)
        {
            idInputFiled.text = idText.text;
        }

        passwordInputField.text = passwordText.text;
        inputSystem.OffAutoPanel();
    }


}
