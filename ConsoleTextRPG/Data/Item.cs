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
        public string Comment { get; set; }
        public ItemType Type { get; set; }
        public string StatType { get; set; }
        public int StatusBonus { get; set; }
        public int Price { get; set; }
        // 아이템이 장착되어 있는지 여부를 추적하는 속성
        public bool IsEquipped { get; set; }

        public Item(int id, string name, ItemType type, int statusBonus, string comment, int price)
        {
            Id = id;
            Name = name;
            Comment = comment;
            Type = type;
            StatusBonus = statusBonus;
            Price = price;
            IsEquipped = false;

            // Type에 따라 StatType 문자열을 자동으로 생성한다.
            if (type == ItemType.Weapon)
            {
                StatType = "공격력";
            }
            else if (type == ItemType.Armor)
            {
                StatType = "방어력";
            }
        }
        public override string ToString()
        {
            return $"{Id}.{Name} | {StatType} + {StatusBonus} | {Comment} | {Price} G";
        }


    }
}
