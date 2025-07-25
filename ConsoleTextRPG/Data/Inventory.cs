﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    public class Inventory
    {
        public List<Item> Items { get; private set; }

        public int PotionCount = 0;

        // 플레이어 객체가 생성될 때 Inventory 객체도 생성하고, Inventory 객체가 생성될 때 비어있는 아이템 리스트를 만들어줌
        public Inventory()
        {
            Items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);
        }

        // 인벤토리를 비우는 메서드
        public void Clear()
        {
            Items.Clear();
        }

        //포션 획득 메서드
        public void AddPotions(int count)
        {
            if (count >= 0)
            {
                PotionCount += count;
            }
        }

        //포션 사용 메서드
        public void UsePotion()
        {
            if (PotionCount > 0)
            {
                PotionCount--;
            }
            else
            {
                Console.WriteLine("물약이 없습니다.");
            }
        }

    }
}
