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
        Player myPlayer = GameManager.Instance.Player;
        //시작
        public override void RenderMenu()
        {
            Display();
        }
        //업데이트
        public override void UpdateInput()
        {
            ItemDisplay();
        }
        //진열할 아이템 추가
        public StoreScene()
        {
            shopItem.Add(new Item(1, "수련자 갑옷    ", Item.ItemType.Armor, 5, "수련에 도움을 주는 갑옷입니다.                    ", 1000));
            shopItem.Add(new Item(2, "무쇠갑옷       ", Item.ItemType.Armor, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.                ", 2000));
            shopItem.Add(new Item(3, "스파르타의 갑옷", Item.ItemType.Armor, 15,"스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ", 3500));
            shopItem.Add(new Item(4, "낡은 검        ", Item.ItemType.Weapon, 2,"쉽게 볼 수 있는 낡은 검 입니다.                   ", 600));
            shopItem.Add(new Item(5, "청동 도끼      ", Item.ItemType.Weapon, 5,"어디선가 사용됐던거 같은 도끼입니다.              ", 1500));
            shopItem.Add(new Item(6, "스파르타의 창  ", Item.ItemType.Weapon, 7,"스파르타의 전사들이 사용했다는 전설의 창입니다.   ", 3000));

        }
        //아이템 상점 소개
        public void Display()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{myPlayer.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            Console.WriteLine();
            for (int i = 0; i < shopItem.Count; i++)
            {
                Console.WriteLine($"{shopItem[i]}");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        //아이템 상점 구경하기
        public void ItemDisplay()
        {
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
                Console.Clear();
                Console.WriteLine("상품 진열중....");
                Thread.Sleep(700);
                ItemBuy();
            }
            else
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("잘못된 입력입니다.");
                Thread.Sleep(700);
            }
        }
        //아이템 구매창
        public void ItemBuy()
        {
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
            else if (int.TryParse(input, out int select))
            {
                if(select >= 1 && select <= shopItem.Count)
                {
                    BuyCycle(input);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(700);
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("잘못된 입력입니다.");
                Thread.Sleep(700);
            }
        }
        //구매 로직
        public void BuyCycle(string select)
        {
            if (int.TryParse(select, out int selectnum))
            {
                selectnum--;

                if (myPlayer.Gold >= shopItem[selectnum].Price)
                {
                    shopItem[selectnum].IsSoldOut = true;
                    Console.WriteLine();
                    Console.WriteLine($"==== {shopItem[selectnum].Id}.{shopItem[selectnum].Name} 구매 완료 ====");
                    //Player.AddGold(shopItem[1].Price);
                    Thread.Sleep(700);
                    ItemBuy();
                }
                else
                {
                    Console.WriteLine("골드가 부족 합니다.");
                    Thread.Sleep(700);
                    ItemBuy();
                }
            }
            else 
            {
                Console.WriteLine("잘못된 입력입니다.");
            }

        }

    }

}
