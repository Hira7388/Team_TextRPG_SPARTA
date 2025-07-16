using ConsoleTextRPG.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Managers
{
    public class QuestManager
    {
        // 싱글톤 패턴
        private static QuestManager _instance;
        public static QuestManager Instance
        {
            get
            {
                if (_instance == null) _instance = new QuestManager();
                return _instance;
            }
        }

        // 모든 퀘스트 정보 받아오기
        public List<Quest> AllQuests { get; private set; }

        // 생성자
        private QuestManager() { }

        // 모든 퀘스트 정보를 Json 파일에서 받아오는 메서드
        public void LoadQuestDatabase()
        {
            // 
            try
            {
                string projectRootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".."));
                string itemDbPath = Path.Combine(projectRootPath, "Json", "quests.json");

                if (File.Exists(itemDbPath))
                {
                    string json = File.ReadAllText(itemDbPath, Encoding.UTF8);
                    this.AllQuests = JsonConvert.DeserializeObject<List<Quest>>(json);
                }
                else
                {
                    this.AllQuests = new List<Quest>();
                    Console.WriteLine("아이템 데이터베이스 파일을 찾을 수 없습니다!");
                }
            }
            catch (Exception ex)
            {
                // JSON 형식 오류 등 예외 발생 시 처리
                Console.WriteLine($"아이템 데이터베이스 로딩 중 오류 발생: {ex.Message}");
                this.AllQuests = new List<Quest>();
            }
        }


        // 몬스터가 처치될 때 호출되는 메서드
        public void OnMonsterKilled(string monsterName)
        {
            Player player = GameManager.Instance.Player;

            // 진행 중인 'Kill' 타입의 퀘스트를 찾습니다.
            foreach (PlayerQuest pq in player.Quests.Where(q => q.State == QuestState.InProgress))
            {
                Quest questInfo = AllQuests.FirstOrDefault(q => q.Id == pq.QuestId);
                if (questInfo != null && questInfo.Type == QuestType.Hunting && questInfo.TargetName == monsterName)
                {
                    pq.CurrentCount++;
                    CheckQuestCompletion(pq);
                }
            }
        }

        // 퀘스트가 완료되었는지 확인 & 이후 처리하는 메서드
        private void CheckQuestCompletion(PlayerQuest playerQuest)
        {
            Player player = GameManager.Instance.Player;
            Quest questInfo = AllQuests.FirstOrDefault(q => q.Id == playerQuest.QuestId);

            if (playerQuest.CurrentCount >= questInfo.TargetCount)
            {
                playerQuest.State = QuestState.Completed;
                Console.WriteLine($"\n[퀘스트 완료: {questInfo.Name}]");

                // 완료 목록에 퀘스트 ID를 추가합니다.
                if (!player.CompletedQuestIds.Contains(questInfo.Id))
                {
                    player.CompletedQuestIds.Add(questInfo.Id);
                }
                // 진행 중 목록에서 해당 퀘스트를 제거합니다.
                player.Quests.Remove(playerQuest);

                // 보상 골드 지급
                player.AddGold(questInfo.RewardGold);

                foreach (int itemId in questInfo.RewardItemIds)
                {
                    Item item = GameManager.Instance.AllItems.FirstOrDefault(i => i.Id == itemId);
                    if (item != null)
                    {
                        player.Inventory.AddItem(item.Clone());
                        Console.WriteLine($"보상으로 {item.Name}을(를) 획득했습니다!");
                    }
                }
            }
        }

    }
}
