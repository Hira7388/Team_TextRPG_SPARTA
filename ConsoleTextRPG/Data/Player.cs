using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    public class Player : Character
    {
        public int Gold { get; private set; }

        // 인벤토리
        public Inventory Inventory { get; private set; }
        // 장착 슬롯
        public Item EquippedWeapon { get; private set; }
        public Item EquippedArmor { get; private set; }

        // 직업 속성
        public string Job { get; private set; }

        // 플레이어 생성자
        public Player(string name)
        {
            // Character(부모)로부터 물려받은 속성 초기화
            this.Name = name;
            // 플레이어의 초기 기본 능력치를 설정합니다. (레벨 1, 공격력 10, 방어력 5, 체력 100)
            this.Stat = new CharacterStat(1, 10, 5, 100);

            // Player만의 고유 속성 초기화
            this.Gold = 1500;

            // Player에게 인벤토리 생성
            this.Inventory = new Inventory();
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
            if (job == "전사")
            {
                // 레벨, 공격력, 방어력, 최대체력
                Stat.SetBaseStats(1, 15, 10, 120);
            }
            else if (job == "마법사")
            {
                Stat.SetBaseStats(1, 20, 5, 80);
            }
        }

        // 불러오기 시 불러온 데이터를 현재 플레이어 객체 데이터에 저장한다.
        public void LoadFromData(SaveData data)
        {
            this.Name = data.PlayerName;
            this.Job = data.PlayerJob;
            this.Gold = data.Gold;
            this.Stat.SetBaseStats(data.Level, data.BaseAttack, data.BaseDefense, data.MaxHp);
            this.Stat.LoadCurrentHp(data.CurrentHp);
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

    }
}
