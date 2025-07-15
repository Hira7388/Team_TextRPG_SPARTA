using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTextRPG.Data;
using ConsoleTextRPG.Managers;
using ConsoleTextRPG.Scenes;

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
            shopItem.Add(new Item(1, "수련자 갑옷    ", Item.ItemType.Armor, 5, "수련에 도움을 주는 갑옷입니다.                    ", 100));
            shopItem.Add(new Item(2, "무쇠갑옷       ", Item.ItemType.Armor, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.                ", 200));
            shopItem.Add(new Item(3, "스파르타의 갑옷", Item.ItemType.Armor, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ", 350));
            shopItem.Add(new Item(4, "낡은 검        ", Item.ItemType.Weapon, 2, "쉽게 볼 수 있는 낡은 검 입니다.                   ", 60));
            shopItem.Add(new Item(5, "청동 도끼      ", Item.ItemType.Weapon, 5, "어디선가 사용됐던거 같은 도끼입니다.              ", 150));
            shopItem.Add(new Item(6, "스파르타의 창  ", Item.ItemType.Weapon, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.   ", 300));
        }

        //아이템 상점 소개
        public void Display()
        {
            Console.Clear();
            Console.WriteLine("=== 상점 ===");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{myPlayer.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            Console.WriteLine();
            for (int i = 0; i < shopItem.Count; i++)
            {
                if (shopItem[i].IsSoldOut)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray; // 판매완료 아이템은 회색으로
                }
                else
                {
                    Console.ResetColor(); // 판매되지 않은 아이템은 기본 색상으로
                }
                Console.WriteLine($"{shopItem[i]}");
            }
            Console.ResetColor(); // 다른 콘솔에 영향을 주지않도록 기본 색상으로
            Console.WriteLine();
            Console.WriteLine();
        }

        //아이템 상점 구경하기
        public void ItemDisplay()
        {
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매"); //미완성
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
                Console.WriteLine("상품 흥정중....");
                Thread.Sleep(700);
                ItemBuy();
            }
            else if (input == "2") //미완성
            {
                Console.Clear();
                Console.WriteLine("인벤토리 여는중....");
                Thread.Sleep(700);
                ItemSell();
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
            while (true)
            {
                Display();
                Console.WriteLine("번호로 아이템 구매 (1 ~ 6)");
                Console.WriteLine();
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
                    if (select >= 1 && select <= shopItem.Count)
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
        }

        //구매 로직
        public void BuyCycle(string select)
        {
            if (int.TryParse(select, out int selectnum))
            {
                selectnum--; //배열은 0부터 시작이니깐

                if (shopItem[selectnum].IsSoldOut == true) //이미 구매한 경우
                {
                    Console.WriteLine("이미 구매한 상품입니다.");
                    Thread.Sleep(700);
                    return;
                }
                else if (myPlayer.Gold < shopItem[selectnum].Price) //골드가 부족할때
                {
                    Console.WriteLine("골드가 부족 합니다.");
                    Thread.Sleep(700);
                    return;
                }
                else //번호에 맞는 상품과 골드 비교
                {
                    shopItem[selectnum].IsSoldOut = true;
                    Console.WriteLine();
                    Console.WriteLine($"==== {shopItem[selectnum].Id}.{shopItem[selectnum].Name} 구매 완료 ====");
                    myPlayer.Inventory.AddItem(shopItem[selectnum]); //아이템 추가
                    myPlayer.AddGold(-shopItem[selectnum].Price); //골드 차감
                    Thread.Sleep(700); 
                    return;
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }

        }

        //아이템 판매창 만드는중..
        public void ItemSell()
        {
            List<Item> playerItems = myPlayer.Inventory.Items;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== 상점 - 아이템 판매 ===");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{myPlayer.Gold} G");
                Console.WriteLine();

                Console.WriteLine("번호로 아이템 판매 (1 ~");
                Console.WriteLine();
                Console.WriteLine();
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
                    select--; // 기본템이 있기 때문에

                    if (select >= 1 && select <= myPlayer.Inventory.Items.Count) //입력값을 비교
                    {
                        int sellPrice = (int)(shopItem[select].Price * 0.8); //원 가격의 80프로
                        myPlayer.AddGold(sellPrice); //골드 더해주기
                        //myPlayer.Inventory.RemoveItem(item[select]); 아이템 제거
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("판매할 아이템이 없습니다.");
                        Thread.Sleep(700);
                        return;
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
        }
    }
}
