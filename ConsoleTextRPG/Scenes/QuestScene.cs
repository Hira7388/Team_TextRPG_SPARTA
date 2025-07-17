using ConsoleTextRPG.Data;
using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Scenes
{
    public class QuestScene : BaseScene
    {
        public enum SceneState
        {
            Main,
            QuestDetails
        }

        // 게임매니저에서 생성된 플레이어 객체 가져오기
        Player questPlayer = GameManager.Instance.Player;

        // 첫 입장시 Main 씬으로 먼저 출력한다.
        private SceneState _currentState = SceneState.Main;

        // 사용자가 선택한 퀘스트의 원본 데이터
        private Quest selectedQuest = null; 

        // 출력 로직
        public override void RenderMenu()
        {
            Print("@ Quest!! @",ConsoleColor.Red);
            Print("");

            // 현재 상태에 따라 다른 화면을 그립니다.
            if (_currentState == SceneState.Main)
            {
                DisplayQuestList();
            }
            else if (_currentState == SceneState.QuestDetails && selectedQuest != null)
            {
                DisplayQuestDetails();
            }

        }

        // 입력 로직
        public override void UpdateInput()
        {
            // 현재 상태에 따라 다른 입력 처리를 합니다.
            if (_currentState == SceneState.Main)
            {
                MainInput();
            }
            else if (_currentState == SceneState.QuestDetails)
            {
                DetailsInput();
            }
        }


        // -------------- 출력 화면
        // 퀘스트 리스트를 출력하는 메서드
        private void DisplayQuestList()
        {
            // 캐릭터가 이미 완료한 퀘스트를 제외하고 수락 가능한 퀘스트 목록들을 List에 저장한다.
            List<Quest> acceptableQuests = QuestManager.Instance.AllQuests.Where(quest => !questPlayer.CompletedQuestIds.Contains(quest.Id)).ToList();

            if (acceptableQuests.Count == 0) Info("수락 가능한 퀘스트가 없습니다.");
            else
            {
                for (int i = 0; i < acceptableQuests.Count; i++)
                {
                    Quest quest = acceptableQuests[i];
                    PlayerQuest playerQuest = questPlayer.Quests.FirstOrDefault(playerQuest => playerQuest.QuestId == quest.Id);

                    if (playerQuest != null) // 이미 진행중인 퀘스트일 경우
                    {
                        Print($"{i + 1}. {quest.Name} (진행 중)", ConsoleColor.Red);
                    }
                    else // 완료한 퀘스트도 아니고 진행중이지도 않은 수락 가능한 퀘스트일 경우
                    {
                        Print($"{i + 1}. {quest.Name}");
                    }
                }
            }
            Print("");
            Print("0. 나가기");
            Print("");
            Print("원하시는 퀘스트를 입력해주세요");
            Console.Write(">> ");
        }

        // 선택한 퀘스트의 디테일한 설명을 출력하는 메서드

        private void DisplayQuestDetails()
        {
            PlayerQuest playerQuest = questPlayer.Quests.FirstOrDefault(playerQuest => playerQuest.QuestId == selectedQuest.Id);
            int currentCount = playerQuest?.CurrentCount ?? 0;

            Print($"{selectedQuest.Name}");
            Print("");
            Print($"{selectedQuest.Description}");
            Print("");
            Print("@ 보상 @", ConsoleColor.Green);
            Print($"{selectedQuest.RewardGold} G");
            foreach (int itemId in selectedQuest.RewardItemIds)
            {
                // 보상 아이템의 이름을 GameManager에서 찾아와 표시합니다.
                Item rewardItem = GameManager.Instance.AllItems.FirstOrDefault(i => i.Id == itemId);
                if (rewardItem != null)
                {
                    Print($"{rewardItem.Name} x 1");
                }
            }
            Print("");
            // 퀘스트 상태에 따라 다른 선택지를 보여줍니다.
            if (playerQuest == null) // 아직 수락 안 한 퀘스트
            {
                Print("1. 수락");
                Print("2. 거절");
            }
            else // 진행 중인 퀘스트
            {
                Print("0. 돌아가기");
            }
            Print("");
            Print("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
        }


        // -------------------- 입력 처리
        // 메인 상태일 때 입력 처리
        private void MainInput()
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice))
            {
                if (choice == 0)
                {
                    GameManager.Instance.SwitchScene(GameState.TownScene);
                    return;
                }

                Player player = GameManager.Instance.Player;
                List<Quest> availableQuests = QuestManager.Instance.AllQuests
                    .Where(q => !player.CompletedQuestIds.Contains(q.Id))
                    .ToList();

                if (choice > 0 && choice <= availableQuests.Count)
                {
                    // 선택된 퀘스트 정보를 저장하고, 상세 보기 상태로 전환합니다.
                    selectedQuest = availableQuests[choice - 1];
                    _currentState = SceneState.QuestDetails;
                }
                else
                {
                    Info("잘못된 번호입니다.");
                    Thread.Sleep(500);
                }
            }
            else
            {
                Info("잘못된 입력입니다.");
                Thread.Sleep(500);
            }
        }

        // 퀘스트 상세보기 상태일 때 입력 처리
        private void DetailsInput()
        {
            PlayerQuest playerQuest = questPlayer.Quests.FirstOrDefault(pq => pq.QuestId == selectedQuest.Id);
            string input = Console.ReadLine();

            if (playerQuest == null) // 아직 수락 안 한 퀘스트일 경우
            {
                if (input == "1") // 1. 수락
                {
                    questPlayer.AcceptQuest(selectedQuest.Id);
                    Info("퀘스트를 수락했습니다.");
                    Thread.Sleep(1000);
                }
                // 거절(2)을 누르거나 다른 키를 눌러도 목록으로 돌아갑니다.
            }

            // 상세 보기 화면에서 어떤 행동을 하든, 다시 목록 보기 상태로 돌아갑니다.
            _currentState = SceneState.Main;
            selectedQuest = null;
        }
    }
}
