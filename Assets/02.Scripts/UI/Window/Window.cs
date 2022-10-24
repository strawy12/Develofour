using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public /*abstract*/ class Window : MonoBehaviour, IPointerClickHandler
{
    private static int windowID = -1;

    [SerializeField]
    protected WindowInfoSO windowInfo;
    #region Property
    public WindowInfoSO Info => windowInfo;
    public Sprite IconSprite => iconImage.sprite;
    public string ID => $"{windowInfo.WindowName}_{myWindowID}";
    #endregion

    #region UI
    protected Button closeBtn;
    /// <summary>
    /// 최소화 버튼
    /// </summary>
    protected Button minimumBtn;
    /// <summary>
    /// 전체화면 버튼
    /// </summary>
    protected Button maximumBtn;

    protected TMP_Text titleText;
    private Image iconImage;
    #endregion

    protected RectTransform rectTransform;
    protected CanvasGroup canvasGroup;

    #region Event
    public Action OnOpen;
    public Action<string> OnClose;
    public Action OnMaximum;
    public Action OnMinimum;
    public Action OnTurnOn;
    public Action OnTurnOff;
    #endregion

    protected bool isOpen = false;
    protected bool isMaximum = false;
    protected int myWindowID;

    protected Vector3 beforePos;
    protected Vector2 beforeSize;

    private void Awake()
    {
        Bind();
        Init();
        Open();
    }

    protected virtual void Bind()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        closeBtn = transform.Find("MenuBar/Btns/CloseBtn").GetComponent<Button>();
        minimumBtn = transform.Find("MenuBar/Btns/MinBtn").GetComponent<Button>();
        maximumBtn = transform.Find("MenuBar/Btns/MaxBtn").GetComponent<Button>();

        titleText = transform.Find("MenuBar/TitleText").GetComponent<TMP_Text>();
        iconImage = transform.Find("MenuBar/IconImage").GetComponent<Image>();
    }

    public virtual void Init()
    {
        if (windowInfo == null)
        {
            Debug.LogError($"{name}'s WindowInfo is null");
            return;
        }

        myWindowID = windowID++;

        titleText.text = windowInfo.WindowName;
        rectTransform.position = windowInfo.Pos;
        rectTransform.sizeDelta = windowInfo.Size;

        closeBtn.onClick.AddListener(Close);
        maximumBtn.onClick.AddListener(MaximumWindow);
        minimumBtn.onClick.AddListener(MinimumWindow);

    }

    public void Open()
    {
        if (isOpen) return;
        //TODO: Tween 적용
        OnOpen?.Invoke();

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        gameObject.SetActive(true);

        isOpen = true;
    }

    public void MaximumWindow()
    {
        //TODO: Tween 적용
        OnMaximum?.Invoke();

        if (isMaximum)
        {

        }

        else
        {
            beforeSize = rectTransform.sizeDelta;
            rectTransform.sizeDelta = Constant.MAXWINSIZE;

            beforePos = rectTransform.position;
            rectTransform.position = Vector3.zero;
        }

        maximumBtn.
    }

    public void MinimumWindow()
    {
        isOpen = false;
        //TODO: Tween 적용
        OnMinimum?.Invoke();

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        gameObject.SetActive(false);

    }

    public void Close()
    {
        // TODO: Tween 적용
        OnClose?.Invoke(ID);
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            WindowManager.Inst.TurnOnWindow(this);
        }
    }
}
