﻿using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTextRPG.Data
{


    public class Player : Character
    {
        //경험치
        public int CurrentExp { get; private set; } 

        //스킬
        public List<Skill> Skills { get; private set; } // Skill 타입 사용


        public int Gold { get;  set; }

        // 인벤토리
        public Inventory Inventory { get; private set; }
        // 장착 슬롯
        public Item EquippedWeapon { get; private set; }
        public Item EquippedArmor { get; private set; }

        // 직업 속성
        public string Job { get; private set; }

        // 퀘스트 속성
        public List<PlayerQuest> Quests { get; private set; } // 현재 수락한 퀘스트 정보
        public List<int> CompletedQuestIds { get; private set; } // 한 번 완료한 퀘스트는 더 이상 출력되지 않음

        // 플레이어 생성자
        public Player(string name)
        {
            // Character(부모)로부터 물려받은 속성 초기화
            this.Name = name;
            // 플레이어의 초기 기본 능력치를 설정합니다. (레벨 1, 공격력 10, 방어력 5, 체력 100, 민첩 5)
            this.Stat = new CharacterStat(1, 10, 5, 100, 5);

            // Player만의 고유 속성 초기화
            this.Gold = 3000;

            // Player에게 인벤토리 생성
            this.Inventory = new Inventory();

            // 수락한 퀘스트와 완료한 퀘스트 정보
            this.Quests = new List<PlayerQuest>();
            this.CompletedQuestIds = new List<int>();
            this.Skills = new List<Skill>(); // 초기 스킬 리스트


        }

        // 이름을 설정하는 전용 메서드
        public void SetName(string name)
        {
            this.Name = name;
        }

        // 직업을 선택하고 그에 맞는 스탯을 설정하는 메서드
        public void SetJob(string job)
        {
            this.Job = job;
            Skills.Clear(); // 기존 스킬 초기화
            if (job == "전사")
            {
                // 레벨, 공격력, 방어력, 최대체력, 민첩
                Stat.SetBaseStats(1, 15, 10, 120, 20);
                Skills.AddRange(SkillManager.Instance.AllSkills.Where(s => s.JobRestriction == "전사"));
            }
            else if (job == "마법사")
            {
                Stat.SetBaseStats(1, 20, 5, 80, 10);
                Skills.AddRange(SkillManager.Instance.AllSkills.Where(s => s.JobRestriction == "마법사"));
            }
        }

        // 스킬 사용 메서드
        public void UseSkill(int skillId, Character target)
        {
            var skill = Skills.FirstOrDefault(s => s.Id == skillId);
            if (skill == null)
            {
                Console.WriteLine("해당 스킬을 찾을 수 없습니다!");
                return;
            }

            skill.Use(this, target);

            if (target.Stat.IsDead)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{target.Name}이(가) 스킬에 의해 쓰러졌습니다!");
                Console.ResetColor();
                Thread.Sleep(250);
            }

        }

        //쿨다운
        public void ReduceSkillCooldowns()
        {
            foreach (var skill in Skills)
            {
                skill.ReduceCooldown();
            }

        }
        
        //턴 엔드
        public void EndTurn()
        {
            foreach (var skill in Skills)
            {
                skill.ReduceCooldown();
            }
        }


        // 불러오기 시 불러온 데이터를 현재 플레이어 객체 데이터에 저장한다.
        public void LoadFromData(SaveData data)
        {
            this.Name = data.PlayerName;
            this.Job = data.PlayerJob;
            this.Gold = data.Gold;
            this.Stat.SetBaseStats(data.Level, data.BaseAttack, data.BaseDefense, data.MaxHp, data.Dexterity);
            this.Stat.LoadCurrentHp(data.CurrentHp);
            this.Quests.Clear();
            if (data.InProgressQuestIds != null) this.Quests.AddRange(data.InProgressQuestIds);

            this.CompletedQuestIds.Clear();
            if (data.CompletedQuestIds != null) this.CompletedQuestIds.AddRange(data.CompletedQuestIds);
        }

        //Gold +하는 메서드
        public void AddGold(int amount)
        {
            int sum = Gold + amount;

            if(sum < 0)
            {
                Gold = 0;
            }
            else
            {
                Gold += amount;
            }
        }

        // 아이템을 장착하는 메서드
        public void EquipItem(Item item)
        {
            if (item.Type == Item.ItemType.Weapon)
            {
                if (this.EquippedWeapon != null) UnequipItem(this.EquippedWeapon);
                this.EquippedWeapon = item;
            }
            else if (item.Type == Item.ItemType.Armor)
            {
                if (this.EquippedArmor != null) UnequipItem(this.EquippedArmor);
                this.EquippedArmor = item;
            }

            // 물약은 장착할 수 없으므로 예외 처리
            else if (item.Type == Item.ItemType.Potion)
            {
                return; // 물약은 장착할 수 없음
            }

            item.IsEquipped = true;
            this.Stat.AddBonusStats(item); // Stat에 보너스 능력치 적용 요청
        }

        // 아이템 장착을 해제하는 메서드
        public void UnequipItem(Item item)
        {
            if (item.Type == Item.ItemType.Weapon) this.EquippedWeapon = null;
            else if (item.Type == Item.ItemType.Armor) this.EquippedArmor = null;

            item.IsEquipped = false;
            this.Stat.RemoveBonusStats(item); // Stat에 보너스 능력치 제거 요청
        }

        // 물약 섭취 메서드 추가
        public void EatPotion(Item potion)
        {
            if (potion.Type == Item.ItemType.Potion)
            {
                // 물약의 효과를 Stat에 적용
                this.Stat.AddHPStats(potion);
                Inventory.RemoveItem(potion);   // 인벤토리 아이템 제거
            }
        }

        // 퀘스트를 수락하는 메서드
        public void AcceptQuest(int questId)
        {
            // 이미 완료했거나 진행중인 퀘스트가 아니라면 추가
            if (!CompletedQuestIds.Contains(questId) && !Quests.Any(playerInProcessQuest => playerInProcessQuest.QuestId == questId))
            {
                Quests.Add(new PlayerQuest(questId));
            }
        }

        //경험치 증가 메서드
        public void GetExp(int exp)
        {
            CurrentExp += exp;

            if(CurrentExp >= Stat.RequiredExp()) //레벨업에 필요한 경험치에 도달할 경우
            {
                Console.WriteLine();
                Console.WriteLine($"{Name}가 {exp}의 exp를 획득하였습니다!");
                CurrentExp -= Stat.RequiredExp(); //사용한 경험치 감소
                Stat.LevelUp(1); // 레벨 1 증가
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"{Name}가 {exp}의 exp를 획득하였습니다!");
            }
        }
    }
}
