using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GuideManager : MonoBehaviour
{
    private void StartGudie(EGuideTopicName guideTopic)
    {
        switch (guideTopic)
        {
            case EGuideTopicName.None:
                break;
            case EGuideTopicName.GuestLoginGuide:
                {
                    MonologSystem.OnStartMonolog.Invoke(EMonologTextDataType.GuestLoginGuideLog, 0.5f, true);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            case EGuideTopicName.LibraryOpenGuide:
                {
                    MonologSystem.OnStartMonolog.Invoke(EMonologTextDataType.GuideLog1, 0.2f, true);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            case EGuideTopicName.ClickPinNotePadHint:
                {
                    EventManager.TriggerEvent(EProfileEvent.ProfileSendMessage, new object[1] { guideTopicDictionary[guideTopic].guideTexts[0] });

                    DataManager.Inst.SetGuide(guideTopic, true);

                    break;
                }
            case EGuideTopicName.ClearPinNotePadQuiz:
                {
                    SendAiChattingGuide(guideTopicDictionary[guideTopic].guideTexts, 0.75f, false);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            case EGuideTopicName.SuspectResidence:
                {
                    if (DataManager.Inst.IsProfileInfoData(EProfileCategory.SuspectProfileInformation, "SuspectIsLivingWithVictim"))
                    {
                        ProfileChattingSystem.OnChatEnd += delegate
                        {
                            MonologSystem.OnStartMonolog?.Invoke(EMonologTextDataType.SuspectResidence, 0.1f, false);
                        };
                        SendProfileGuide();
                    }
                    else
                    {
                        SendAiChattingGuide("용의자 거주지에 대한 정보를 찾지 못했습니다. 죄송합니다", false);
                    }
                    break;
                }
            case EGuideTopicName.VictimBirthDate:
                {
                    if (DataManager.Inst.SaveData.isOnceOpenWindowProperty)
                    {
                        SendProfileGuide();
                    }
                    else
                    {
                        SendAiChattingGuide(guideTopicDictionary[guideTopic].guideTexts, 0.75f, false);
                    }
                    break;
                }
            case EGuideTopicName.VictimUniversity:
                {
                    if (DataManager.Inst.IsProfileInfoData(EProfileCategory.VictimProfileInformation, "VictimName"))
                    {
                        SendProfileGuide();
                    }
                    else
                    {
                        SendAiChattingGuide("피해자 대학교에 대한 정보를 찾지 못했습니다. 죄송합니다", false);
                    }
                    break;

                }
            case EGuideTopicName.PetAdoptionDate:
                {
                    if (DataManager.Inst.SaveData.isOnceOpenWindowProperty)
                    {
                        SendProfileGuide();
                    }
                    else
                    {
                        SendAiChattingGuide(guideTopicDictionary[guideTopic].guideTexts, 0.75f, false);
                    }
                    break;
                }
            case EGuideTopicName.SuspectIsLivingWithVictim:
            case EGuideTopicName.VictimJob:
            case EGuideTopicName.PetName:
            case EGuideTopicName.PetAge:
            case EGuideTopicName.SuspectRelationWithVictim:
            case EGuideTopicName.SuspectBirth:
            case EGuideTopicName.SuspectEmail:
            case EGuideTopicName.SuspectUniversity:
            case EGuideTopicName.SuspectDepartment:
            case EGuideTopicName.SuspectIsPetHaveAnswer:
            case EGuideTopicName.VictimName:
            case EGuideTopicName.VictimDeathTime:
            case EGuideTopicName.PetBreed:
                {
                    SendProfileGuide();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
