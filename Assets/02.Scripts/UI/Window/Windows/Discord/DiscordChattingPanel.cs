using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscordChattingPanel : MonoBehaviour
{ 
    [SerializeField]
    private TMP_Text stateText;

    private DiscordProfileDataSO playerProfile;
    private DiscordProfileDataSO userProfile;

    public IEnumerator WaitingTypingCoroutine(float delay)
    {
        stateText.text = $"{userProfile.userName}���� �Է��ϰ� �־��...";
        yield return new WaitForSeconds(delay);
    }
    

}
