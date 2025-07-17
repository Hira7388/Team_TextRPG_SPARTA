using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    public class Item
    {
        public enum ItemType
        {
            Weapon,
            Armor
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public int StatusBonus { get; set; }
        public string Comment { get; set; }
        public int Price { get; set; }
        // 아이템이 장착되어 있는지 여부를 추적하는 속성

        // 아래 속성은 플레이어가 소유한 복사본의 상태를 나타낸다. (아이템 자체의 원본 정보가 바뀌면 안된다.)
        public bool IsEquipped { get; set; }
        public string StatType => Type == ItemType.Weapon ? "공격력" : "방어력";

        // JSON 라이브러리가 사용하는 기본 생성자
        public Item() { }

        // 코드에서 아이템을 생성할 때 사용하는 생성자
        public Item(int id, string name, ItemType type, int statusBonus, string comment, int price)
        {
            Id = id;
            Name = name;
            Type = type;
            StatusBonus = statusBonus;
            Comment = comment;
            Price = price;
            IsEquipped = false; // 생성 시에는 항상 false로 초기화
        }

        public Item Clone() => (Item)this.MemberwiseClone();
    }
}
