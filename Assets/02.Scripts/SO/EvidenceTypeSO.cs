using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EvidenceType")]
public class EvidenceTypeSO : ResourceSO
{
    public int maxCount; // 몇번 틀려야지 정답을 알려줄건가  0 == 안알려줌
    public string selectMonolog; // 제시했을때 나오는 독백
    public string wrongMonolog; // 틀렸을때 나오는 독백
    public string wrongHintMonolog; //틀렸고 Maxcount가 찼을떄 나오는 독백
    public List<string> answerInfoID;
}
