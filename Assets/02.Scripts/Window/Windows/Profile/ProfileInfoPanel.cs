using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProfileInfoPanel : MonoBehaviour
{
    private ProfileCategoryDataSO currentData;
    [SerializeField]
    private TMP_Text titleText;
    [SerializeField]
    private Image categoryImage;
    [SerializeField]
    private RectTransform categoryImageParent;
    [SerializeField]
    private ProfileInfoText infoTextPrefab;
    [SerializeField]
    private Transform poolParent;
    [SerializeField]
    private Transform infoTextParent;
    private Queue<ProfileInfoText> infoTextQueue;

    private List<ProfileInfoText> infoTextList;

    #region Pool
    private void CreatePool()
    {
        for (int i = 0; i < 20; i++)
        {
            ProfileInfoText infoText = Instantiate(infoTextPrefab, poolParent);
            infoText.Init();
            infoText.Hide();
            infoTextQueue.Enqueue(infoText);
        }
    }

    private ProfileInfoText Pop()
    {
        if (infoTextQueue.Count == 0)
        {
            CreatePool();
        }

        ProfileInfoText infoText = infoTextQueue.Dequeue();
        infoText.transform.SetParent(infoTextParent);
        infoTextList.Add(infoText);
        return infoText;
    }

    public void Push(ProfileInfoText infoText)
    {
        if (infoTextList.Contains(infoText))
        {
            infoText.Hide();
            infoText.transform.SetParent(poolParent);
            infoTextList.Remove(infoText);
            infoTextQueue.Enqueue(infoText);
        }
    }
    public void PushAll()
    {
        foreach (var infoText in infoTextList)
        {
            infoTextQueue.Enqueue(infoText);
            infoText.transform.SetParent(poolParent);
            infoText.Hide();
        }
        infoTextList.Clear();
    }
    #endregion

    //private Image currentImage;


    public void Init()
    {

        infoTextQueue = new Queue<ProfileInfoText>();
        infoTextList = new List<ProfileInfoText>();
        //currentImage = GetComponent<Image>();
        //currentImage.material = Instantiate(currentImage.material);
        CreatePool();
        EventManager.StartListening(EProfileEvent.ShowInfoPanel, Show);
        EventManager.StartListening(EProfileEvent.HideInfoPanel, Hide);

        Hide();
    }


    public void ChangeValue(EProfileCategory category, int id)
    {
        if(currentData == null)
        {
            return;
        }
        if (category != currentData.category)
        {
            return;
        }

        foreach (var infoText in infoTextList)
        {
            if (infoText.InfoData.id == id)
            {
                infoText.Show();
            }
        }
    }

    public void Show(object[] ps)
    {
        if (!(ps[0] is ProfileCategoryDataSO))
        {
            return;
        }

        EventManager.TriggerEvent(EProfileEvent.HideInfoPanel);
        EventManager.StartListening(EProfileEvent.Maximum, RefreshSize);
        EventManager.StartListening(EProfileEvent.Minimum, RefreshSize);

        this.gameObject.SetActive(true);
        ProfileCategoryDataSO categoryData = ps[0] as ProfileCategoryDataSO;
        currentData = categoryData;
        titleText.SetText(currentData.categoryName);
        SpriteSetting();

        if(currentData.defaultInfoText != null)
        {
            ProfileInfoText infoText = Pop();
            infoText.Setting(currentData.defaultInfoText);
            infoText.Show();
            infoText.gameObject.SetActive(true);
        }

        foreach (var infoData in currentData.infoTextList)
        {
            ProfileInfoText infoText = Pop();
            infoText.Setting(infoData);

            if (DataManager.Inst.IsProfileInfoData(infoText.InfoData.id))
            {
                infoText.Show();
                infoText.gameObject.SetActive(true);
            }
        }
        StartCoroutine(RefreshSizeCoroutine());

    }
    private void RefreshSize(object[] ps)
    {
        StartCoroutine(RefreshSizeCoroutine());
    }
    private IEnumerator RefreshSizeCoroutine()
    {
        foreach (var infoText in infoTextList)
        {
            infoText.RefreshSize();
        }
        yield return new WaitForSeconds(0.02f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)infoTextParent);
    }
    public void SpriteSetting()
    {
        Define.SetSprite(categoryImage, currentData.categorySprite, categoryImageParent.sizeDelta);
    }
    public void Hide(object[] ps = null)
    {
        gameObject.SetActive(false);
        PushAll();
        EventManager.StopListening(EProfileEvent.Maximum, RefreshSize);
        EventManager.StopListening(EProfileEvent.Minimum, RefreshSize);
    }
    public bool GetIsFindAll()
    {
        foreach (var info in infoTextList)
        {
            if (info.IsFind == false)
            {
                return false;
            }
        }
        return true;
    }

    public string SetInfoText(int id)
    {
        string answer = "";
        foreach (var infoText in infoTextList)
        {
            if (id == infoText.InfoData.id)
            {
                answer = infoText.InfoData.noticeText;
            }
        }

        return answer;
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.ShowInfoPanel, Show);
        EventManager.StopListening(EProfileEvent.HideInfoPanel, Hide);
        EventManager.StopListening(EProfileEvent.Maximum, RefreshSize);
        EventManager.StopListening(EProfileEvent.Minimum, RefreshSize);

    }
    //private void FillPostItColor()
    //{
    //DOTween.To(
    //    () => currentImage.material.GetFloat("_Dissolve"),
    //    (v) => currentImage.material.SetFloat("_Dissolve", v),
    //    1f, 3f);
    //}
}