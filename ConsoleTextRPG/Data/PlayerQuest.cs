using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    // 플레이어가 수락한 퀘스트 관련 데이터
    public enum QuestState
    {
        Completed, // 퀘스트 완료 상태
        InProgress // 퀘스트 진행 상태
    }

    // 플레이어가 받은 퀘스트의 진행 상태 (퀘스트 객체 1개)
    public class PlayerQuest
    {
        public int QuestId { get; set; }
        public int CurrentCount { get; set; }
        public QuestState State { get; set; }

        public PlayerQuest(int questId)
        {
            QuestId = questId;
            CurrentCount = 0;
            State = QuestState.InProgress;
        }
    }
}
