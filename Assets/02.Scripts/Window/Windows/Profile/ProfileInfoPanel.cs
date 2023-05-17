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
    private Transform infoTextParent;
    private Queue<ProfileInfoText> infoTextQueue;

    private List<ProfileInfoText> infoTextList;

    #region Pool
    private void CreatePool()
    {
        for (int i = 0; i < 20; i++)
        {
            ProfileInfoText infoText = Instantiate(infoTextPrefab, infoTextParent);
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
        infoTextList.Add(infoText);
        return infoText;
    }

    public void Push(ProfileInfoText infoText)
    {
        if (infoTextList.Contains(infoText))
        {
            infoText.Hide();
            infoTextList.Remove(infoText);
            infoTextQueue.Enqueue(infoText);
        }
    }
    public void PushAll()
    {
        foreach (var infoText in infoTextList)
        {
            infoTextQueue.Enqueue(infoText);
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


    public void ChangeValue(EProfileCategory category, string key)
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
            if (infoText.InfoData.key == key)
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
        this.gameObject.SetActive(true);
        ProfileCategoryDataSO categoryData = ps[0] as ProfileCategoryDataSO;
        currentData = categoryData;
        titleText.SetText(currentData.categoryName);
        SpriteSetting();
        foreach (var infoData in currentData.infoTextList)
        {
            ProfileInfoText infoText = Pop();
            infoText.Setting(infoData);

            if (DataManager.Inst.IsProfileInfoData(currentData.category, infoText.InfoData.key))
            {
                infoText.Show();
                infoText.gameObject.SetActive(true);
            }
        }
    }
    public void SpriteSetting()
    {
        Define.SetSprite(categoryImage, currentData.categorySprite, categoryImageParent.sizeDelta);
    }
    public void Hide(object[] ps = null)
    {
        gameObject.SetActive(false);
        PushAll();
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

    public string SetInfoText(string key)
    {
        string answer = "";
        foreach (var infoText in infoTextList)
        {
            if (key == infoText.InfoData.key)
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

    }
    //private void FillPostItColor()
    //{
    //DOTween.To(
    //    () => currentImage.material.GetFloat("_Dissolve"),
    //    (v) => currentImage.material.SetFloat("_Dissolve", v),
    //    1f, 3f);
    //}
}