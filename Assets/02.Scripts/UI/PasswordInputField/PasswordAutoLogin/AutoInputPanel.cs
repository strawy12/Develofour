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

    [SerializeField]
    private AutoInput inputSystem;

    public void Setting(TMP_InputField inputField, string answer)
    {
        passwordInputField = inputField;
        passwordText.text = answer;

        gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        passwordInputField.text = passwordText.text;

        inputSystem.OffAutoPanel();
    }
}
