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
                    //MonologSystem.OnStartMonolog.Invoke(EMonologTextDataType.GuestLoginGuideLog, 0.5f);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            case EGuideTopicName.LibraryOpenGuide:
                {
                    //MonologSystem.OnStartMonolog.Invoke(EMonologTextDataType.GuideLog1, 0.2f);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            case EGuideTopicName.ClickPinNotePadHint:
                {
                    EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { guideTopicDictionary[guideTopic].guideTexts[0] });
                    DataManager.Inst.SetGuide(guideTopic, true);

                    break;
                }
            case EGuideTopicName.ClearPinNotePadQuiz:
                {
                    StartCoroutine(SendAiMessageTexts(guideTopicDictionary[guideTopic].guideTexts));
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }

            case EGuideTopicName.SuspectResidence:
                {
                    if (DataManager.Inst.IsProfileInfoData(EProfileCategory.SuspectProfileInformation, "SuspectIsLivingWithVictim"))
                    {
                        ProfileChattingSystem.OnChatEnd += delegate
                        {
                            //MonologSystem.OnStartMonolog?.Invoke(EMonologTextDataType.SuspectResidence, 0.1f);
                        };
                        SendProfileGuide(guideTopic);
                    }
                    else
                    {
                        EventManager.TriggerEvent(EProfileEvent.SendGuide, new object[1] { EAIChattingTextDataType.SuspectResidenceFailed });
                    }
                    break;
                }

            case EGuideTopicName.VictimBirthDate:
                {
                    if (DataManager.Inst.SaveData.isOnceOpenWindowProperty)
                    {
                        SendProfileGuide(guideTopic);
                    }
                    else
                    {
                        EventManager.TriggerEvent(EProfileEvent.SendGuide, new object[1] { EAIChattingTextDataType.VictimBirthDateElse });
                    }
                    break;

                }

            case EGuideTopicName.VictimUniversity:
                {
                    if (DataManager.Inst.IsProfileInfoData(EProfileCategory.VictimProfileInformation, "VictimName"))
                    {
                        SendProfileGuide(guideTopic);
                    }
                    else
                    {
                        EventManager.TriggerEvent(EProfileEvent.SendGuide, new object[1] { EAIChattingTextDataType.VictimUniversityFailed });
                    }
                    break;

                }

            case EGuideTopicName.PetAdoptionDate:
                {
                    if (DataManager.Inst.SaveData.isOnceOpenWindowProperty)
                    {
                        SendProfileGuide(guideTopic);

                    }
                    else
                    {
                        EventManager.TriggerEvent(EProfileEvent.SendGuide, new object[1] { EAIChattingTextDataType.PetAdoptionDateElse });
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
                    SendProfileGuide(guideTopic);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
