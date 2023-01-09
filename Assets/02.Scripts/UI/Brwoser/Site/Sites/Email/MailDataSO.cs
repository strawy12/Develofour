using ExtenstionMethod;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/Mail/Data")]
public class MailDataSO : ScriptableObject
{
    [SerializeField]
    private EMailType type;

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
    public string Time => $"{month}¿ù{day}ÀÏ";
    public int Day => day;
    public int Month => month;
    public bool isFavorited { get { return mailCategory.ContainMask((int)EEmailCategory.Favorite); } }

}
