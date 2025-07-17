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
            Armor,
            Potion
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public string StatType { get; set; }
        public int StatusBonus { get; set; }
        public string Comment { get; set; }
        public int Price { get; set; }
        // 아이템이 장착되어 있는지 여부를 추적하는 속성

        // 아래 속성은 플레이어가 소유한 복사본의 상태를 나타낸다. (아이템 자체의 원본 정보가 바뀌면 안된다.)
        public bool IsEquipped { get; set; }

        // Json 파일을 객체로 변환할 때 사용하는 생성자이다.
        // 빈 객체를 먼저 만들고 Json에 있는 정보를 넣는다.
        public Item() { }

        // 실제 아이템을 생성할 때 사용하는 생성자.
        public Item(int id, string name, ItemType type, int statusBonus, string comment, int price, bool isEquipped)
        {
            Id = id;
            Name = name;
            Type = type;
            StatusBonus = statusBonus;
            Comment = comment;
            Price = price;
            IsEquipped = isEquipped;


            // Type에 따라 StatType 문자열을 자동으로 생성한다.
            if (type == ItemType.Weapon)
            {
                StatType = "공격력";
            }
            else if (type == ItemType.Armor)
            {
                StatType = "방어력";
            }
            else if (type == ItemType.Potion)
            {
                StatType = "회복력";
            }
        }
        public Item Clone()
        {
            return (Item)this.MemberwiseClone();
        }
    }
}
