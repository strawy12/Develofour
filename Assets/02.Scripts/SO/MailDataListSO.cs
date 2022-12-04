using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/List/MailDataList")]
public class MailDataListSO : ScriptableObject
{
    [SerializeField]
    private List<MailData> mailDataList;

    public List<MailData> MailDataList => mailDataList.ToList();
}
