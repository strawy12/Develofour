using ExtenstionMethod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/Mail/Data")]
public class MailDataSO : SOParent
{
    public EMailType type;

    [SerializeField]
    private string nameText;
    [SerializeField]
    private string informationText;
    [SerializeField]
    private int month;
    [SerializeField]
    private int day;
    [SerializeField]
    private string timeText;
    [BitMask(typeof(EEmailCategory))]
    public int mailCategory;

    public EMailType Type => type;
    public string Name => nameText;
    public string Info => informationText;
    public string Time => $"{month}월{day}일";
    public int Day => day;
    public int Month => month;
    public bool isFavorited { get { return mailCategory.ContainMask((int)EEmailCategory.Favorite); } }

    
    public override void Setting(string[] ps)
    {
#if UNITY_EDITOR
        try
        {
            type = (EMailType)Enum.Parse(typeof(EMailType), ps[1]);
        }
        catch
        {
            string PATH = "Assets/02.Scripts/UI/Browser/Site/Sites/Email/EmailSite.cs";
            SOEditorCodeUtill.AddEnum(PATH, ps[1]);

            type = (EMailType)Enum.Parse(typeof(EMailType), ps[1]);
        }
        nameText = ps[2];
        informationText = ps[3];
        month = int.Parse(ps[4]);
        day = int.Parse(ps[5]);
        timeText = $"{ps[4]}월{ps[5]}일";

        //mailCategory = int.Parse(ps[6]);

#endif
    }
}
