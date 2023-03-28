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
                    EventManager.TriggerEvent(EProfileEvent.ProfileSendMessage, new object[1] { guideTopicDictionary[guideTopic].guideTexts[0] });

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
                            MonologSystem.OnStartMonolog?.Invoke(EMonologTextDataType.SuspectResidence, 0.1f, Define.CheckGameState(EGameState.Tutorial));
                        };
                        SendProfileGuide();
                    }
                    else
                    {
                        SendAiChattingGuide("용의자 거주지에 대한 정보를 찾지 못했습니다. 죄송합니다", false);
                        //EventManager.TriggerEvent(EProfileEvent.SendGuide, new object[1] { EAIChattingTextDataType.SuspectResidenceFailed });
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
                        List<string> strList = new List<string>();
                        strList.Add("정보를 탐색해본 결과 피해자 생년월일은 여친 생일 파일 에서 획득 가능합니다.");
                        strList.Add("또한 파일을 우클릭 한다면 속성 창을 열 수 있습니다.");

                        SendAiChattingGuide(strList,0.75f, false);

                        //EventManager.TriggerEvent(EProfileEvent.SendGuide, new object[1] { EAIChattingTextDataType.VictimBirthDateElse });
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
                        //EventManager.TriggerEvent(EProfileEvent.SendGuide, new object[1] { EAIChattingTextDataType.VictimUniversityFailed });
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
                        List<string> strList = new List<string>();
                        strList.Add("정보를 탐색해본 결과 반려동물 첫 입양일은 처음 만난 날 파일 에서 획득 가능합니다.");
                        strList.Add("또한 파일을 우클릭 한다면 속성 창을 열 수 있습니다.");

                        SendAiChattingGuide(strList, 0.75f, false);

                        //EventManager.TriggerEvent(EProfileEvent.SendGuide, new object[1] { EAIChattingTextDataType.PetAdoptionDateElse });
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
