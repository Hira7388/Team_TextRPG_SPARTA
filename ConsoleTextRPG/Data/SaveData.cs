﻿using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int Dexterity { get; set; }

        // 인벤토리 및 장비 정보 (아이템 ID만 저장)
        public List<int> InventoryItemIds { get; set; }
        public int EquippedWeaponId { get; set; } = -1; // 장착 안했을 경우 -1
        public int EquippedArmorId { get; set; } = -1;  // 장착 안했을 경우 -1

        // 던전 클리어 계층 관련 정보

        // 플레이어가 진행중인 퀘스트 관련 정보
        public List<PlayerQuest> InProgressQuestIds { get; set; } // 진행 중인 퀘스트 ID 목록
        public List<int> CompletedQuestIds { get; set; } // 완료한 퀘스트 ID 목록
        //public Dictionary<int, int> QuestProgress { get; set; } // 퀘스트 ID, 현재 카운트 개수
        public List<int> LearnedSkillIds { get; set; }
    }
}
