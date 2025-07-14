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
        // public List<Item> Inventory { get; private set; }

        // 장착 슬롯
        public Item EquippedWeapon { get; private set; }
        public Item EquippedArmor { get; private set; }

        // 플레이어 생성자
        public Player(string name)
        {
            // 1. Character(부모)로부터 물려받은 속성 초기화
            this.Name = name;
            // 플레이어의 초기 기본 능력치를 설정합니다. (레벨 1, 공격력 10, 방어력 5, 체력 100)
            this.Stat = new CharacterStat(1, 10, 5, 100);

            // 2. Player만의 고유 속성 초기화
            this.Gold = 1500;
            //this.Inventory = new List<Item>();
        }
    }
}
