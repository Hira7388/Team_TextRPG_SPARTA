using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTextRPG.Data;
using ConsoleTextRPG.Managers;
using ConsoleTextRPG.Scene;

namespace ConsoleTextRPG.Scenes
{
    public class StoreScene : BaseScene
    {
        List<Item> shopItem = new List<Item>();

        public override void Render()
        {
            Display();
        }

        public override void Update()
        {
            ItemDisplay();
        }

        public StoreScene()
        {
            shopItem.Add(new Item(1, "수련자 갑옷    ", Item.ItemType.Armor, 5, "수련에 도움을 주는 갑옷입니다.                    ", 1000));
            shopItem.Add(new Item(2, "무쇠갑옷       ", Item.ItemType.Armor, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.                ", 2000));
            shopItem.Add(new Item(3, "스파르타의 갑옷", Item.ItemType.Armor, 15,"스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ", 3500));
            shopItem.Add(new Item(4, "낡은 검        ", Item.ItemType.Weapon, 2,"쉽게 볼 수 있는 낡은 검 입니다.                   ", 600));
            shopItem.Add(new Item(5, "청동 도끼      ", Item.ItemType.Weapon, 5,"어디선가 사용됐던거 같은 도끼입니다.              ", 1500));
            shopItem.Add(new Item(6, "스파르타의 창  ", Item.ItemType.Weapon, 7,"스파르타의 전사들이 사용했다는 전설의 창입니다.   ", 3000));

        }
        public void Display()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($" G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            Console.WriteLine();
            for (int i = 0; i < shopItem.Count; i++)
            {
                Console.WriteLine($"{shopItem[i].Id}.{shopItem[i].Name} | {shopItem[i].StatType} + {shopItem[i].StatusBonus,2} | {shopItem[i].Comment} | {shopItem[i].Price,6} G");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        public void ItemDisplay()
        {
            //StoreScene myShop = new StoreScene();
            Display();
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
            string input = Console.ReadLine();
            if (input == "0")
            {
                GameManager.Instance.SwitchScene(GameState.TownScene);
                return;
            }
            else if (input == "1")
            {
                ItemBuy();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
                Console.WriteLine();
            }
        }
        public void ItemBuy()
        {
            //StoreScene myShop = new StoreScene();
            Display();
            Console.WriteLine("번호로 아이템 구매 (1 ~ 6)");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
            string input = Console.ReadLine();
            if (input == "0")
            {
                return;
            }
            else if (input == "1")
            {
                Console.Clear();

            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
                Console.WriteLine();
            }
        }

    }

}
