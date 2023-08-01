using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    public CharacterInfoDataSO FindCharacterPhoneNumber(string phoneNum)
    {
        List<CharacterInfoDataSO> characterList = resourceComponets[typeof(CharacterInfoDataSO)].GetRsourceDictionary().Select(x => x.Value as CharacterInfoDataSO).ToList();
        CharacterInfoDataSO data = characterList.Find(x => x.phoneNum == phoneNum);
        return data;
    }
}
