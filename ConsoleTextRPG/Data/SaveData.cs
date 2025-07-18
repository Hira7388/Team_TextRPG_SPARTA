using System;
using System.Collections.Generic;

namespace ConsoleTextRPG.Data
{
    public class SaveData
    {
        // 플레이어 기본 정보
        public string PlayerName { get; set; }
        public string PlayerJob { get; set; }
        public int Gold { get; set; }

        // 플레이어 스텟 정보
        public int Level { get; set; }
        public int BaseAttack { get; set; }
        public int BaseDefense { get; set; }
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }
        public int MaxMp { get; set; }
        public int CurrentMp { get; set; }
        public int Dexterity { get; set; }

        // 인벤토리 및 장비 정보 (아이템 ID만 저장)
        public List<int> InventoryItemIds { get; set; }
        public int EquippedWeaponId { get; set; } = -1; // 장착 안했을 경우 -1
        public int EquippedArmorId { get; set; } = -1;  // 장착 안했을 경우 -1

        // 던전 클리어 계층 관련 정보

        // 플레이어가 진행중인 퀘스트 관련 정보
        public List<PlayerQuest> InProgressQuestIds { get; set; } // 진행 중인 퀘스트 상태 목록
        public List<int> CompletedQuestIds { get; set; } // 완료한 퀘스트 ID 목록
    }

    // 플레이어의 퀘스트 진행 상태를 저장하는 클래스
    public class PlayerQuest
    {
        public int QuestId { get; set; }      // 퀘스트 ID
        public int CurrentCount { get; set; } // 현재 달성한 목표 수량
        public int State { get; set; }        // 퀘스트 상태 (예: 0=진행전, 1=진행중, 2=완료 등)
    }
}
