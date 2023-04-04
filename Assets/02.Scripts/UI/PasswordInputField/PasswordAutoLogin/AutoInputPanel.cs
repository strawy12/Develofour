using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class AutoInputPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TMP_Text passwordText;

    private TMP_InputField passwordInputField;

    private bool isHaveID;

    [SerializeField]
    private AutoInputSystem inputSystem;

    public void Setting(List<TMP_InputField> inputFields, AutoAnswerData data)
    {
        if(inputFields.Count < 2)
        {
            passwordInputField = inputFields[0];
            passwordText.text = data.answer;  
            isHaveID = false;
        }
        else
        {
            passwordInputField = inputFields[1];
            passwordText.text = data.answer;
            isHaveID = true;
        }

        gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        passwordInputField.text = passwordText.text;
        inputSystem.OffAutoPanel();
    }


}
