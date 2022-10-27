using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public /*abstract*/ class Window : MonoBehaviour, IPointerClickHandler, ISelectable
{
    private static int windowID = -1;

    [SerializeField]
    protected WindowDataSO windowData;
    #region Property
    public WindowDataSO WindowData => windowData;
    public string ID => $"{windowData.WindowName}_{myWindowID}";
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
    protected MaximumBtn maximumBtn;

    protected TMP_Text titleText;
    private Image iconImage;
    #endregion

    public RectTransform rectTransform { get; protected set; }
    protected CanvasGroup canvasGroup;

    #region Event
    public Action OnOpen;
    public Action<string> OnClose;
    public Action OnMaximum;
    public Action OnMinimum;
    public Action OnSelected { get; set; }
    public Action OnUnSelected { get; set; }
    #endregion

    protected bool isOpen = false;
    protected bool isMaximum = false;
    protected int myWindowID;

    protected Vector3 beforePos;
    protected Vector2 beforeSize;

    public void CreateWindow()
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
        maximumBtn = transform.Find("MenuBar/Btns/MaxBtn").GetComponent<MaximumBtn>();

        titleText = transform.Find("MenuBar/TitleText").GetComponent<TMP_Text>();
        iconImage = transform.Find("MenuBar/IconImage").GetComponent<Image>();
    }

    public virtual void Init()
    {
        if (windowData == null)
        {
            Debug.LogError($"{name}'s WindowInfo is null");
            return;
        }

        myWindowID = windowID++;

        iconImage.sprite = windowData.IconSprite;
        titleText.text = $"{windowData.WindowName} - {windowData.Title}";
        rectTransform.position = windowData.Pos;
        rectTransform.sizeDelta = windowData.Size;

        closeBtn.onClick.AddListener(Close);
        maximumBtn.onClick.AddListener(MaximumWindow);
        minimumBtn.onClick.AddListener(MinimumWindow);

        transform.Find("MenuBar").GetComponent<WindowBar>().Init(this);

        EventManager.TriggerEvent(EEvent.CreateWindow, this);
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
        WindowManager.Inst.SelectObject(this);

        isOpen = true;
    }

    public void MaximumWindow()
    {
        //TODO: Tween 적용
        OnMaximum?.Invoke();

        if (isMaximum)
        {
            rectTransform.sizeDelta = beforeSize;
            rectTransform.position = beforePos;

            maximumBtn.iconImage.SetText("□");
            isMaximum = false;
        }

        else
        {
            beforeSize = rectTransform.sizeDelta;
            rectTransform.sizeDelta = Constant.MAXWINSIZE;

            beforePos = rectTransform.position;
            rectTransform.position = Vector3.zero;

            maximumBtn.iconImage.SetText("■");
            isMaximum = true;
        }

    }

    public void MinimumWindow()
    {
        isOpen = false;
        //TODO: Tween 적용
        OnMinimum?.Invoke();

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        WindowManager.Inst.SelectedObjectNull();
        gameObject.SetActive(false);

    }

    public void Close()
    {
        // TODO: Tween 적용
        OnClose?.Invoke(ID);
        WindowManager.Inst.SelectedObjectNull();
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectWindow();
    }

    public void SelectWindow()
    {
        WindowManager.Inst.SelectObject(this);
    }
}
