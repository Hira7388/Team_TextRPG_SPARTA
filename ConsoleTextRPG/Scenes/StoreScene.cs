using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ConsoleTextRPG.Data;
using ConsoleTextRPG.Managers;
using ConsoleTextRPG.Scenes;

namespace ConsoleTextRPG.Scenes
{
    public class StoreScene : BaseScene
    {
        public enum StoreMode
        {
            Main,   // 메인 메뉴 모드
            Buying, // 구매 모드
            Selling // 판매 모드
        }

        private StoreMode _currentMode = StoreMode.Main; // 첫 시작은 메인 메뉴 모드로 시작한다.
        Player myPlayer = GameManager.Instance.Player; // 플레이어 객체를 받아온다.
        List<Item> allItems = GameManager.Instance.AllItems; // 모든 아이템 정보를 받아온다.

        private float _storeDiscountRate = 0.85f; //할인율
        private int _Width = 18;                   //이름 너비 제한
        private int _statWidth = 12;                //스텟 너비 제한
        private int _commentWidth = 50;              //설명 너비 제한
        private int _priceWidth = 5;                  //가격 너비 제한

        // 화면 출력
        public override void RenderMenu()
        {
            Display();
        }
        // 사용자 입력에 따른 액션
        public override void UpdateInput()
        {
            ItemDisplay();
        }

        // ------------------------------ 여기는 화면 출력을 담당
        //아이템 상점 소개 출력
        public void Display()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{myPlayer.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            Console.WriteLine();

            switch (_currentMode) // 현재 모드에 맞는 아이템 목록을 출력한다.
            {
                case StoreMode.Selling:
                    // 판매 모드에서는 플레이어의 인벤토리를 보여줍니다.
                    DisplayPlayerItemsForSale();
                    break;
                default:
                    // 메인 또는 구매 모드에서는 상점의 전체 아이템을 보여줍니다.
                    DisplayStoreItems(_currentMode == StoreMode.Buying);
                    break;
            }

            switch (_currentMode) // 현재 모드에 맞는 입력 요구 화면을 출력한다.
            {
                case StoreMode.Main: // 메인 모드일 때
                    Console.WriteLine();
                    Console.WriteLine("1. 아이템 구매");
                    Console.WriteLine("2. 아이템 판매");
                    Console.WriteLine("0. 나가기\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    Console.Write(">> ");
                    break;
                case StoreMode.Buying: // 구매 모드일 때
                    Info("구매할 아이템의 번호를 입력해주세요. (취소: 0)");
                    Console.Write(">> ");
                    break;
                case StoreMode.Selling: // 판매 모드일 때
                    // TODO: 판매 모드 UI
                    Info("판매할 아이템의 번호를 입력해주세요. (취소: 0)");
                    Console.Write(">> ");
                    break;
            }
        }

        // 메인 or 구매 모드일 때 상점 아이템을 출력하는 모드 (상점의 아이템을 출력함)
        public void DisplayStoreItems(bool showNumbers)
        {
            for (int i = 0; i < allItems.Count; i++)
            {
                Item storeItem = allItems[i];
                bool isSoldOut = myPlayer.Inventory.Items.Any(item => item.Id == storeItem.Id);
                string priceDisplay = isSoldOut ? "구매완료" : $"{storeItem.Price} G";

                Console.ForegroundColor = isSoldOut ? ConsoleColor.DarkGray : ConsoleColor.White;

                if (showNumbers) // 구매 모드일 경우 아이템 앞에 번호를 출력한다.
                {
                    // 번호 표시
                    //Console.Write($"- {i + 1}. {storeItem.Name,-15}");
                    ConsoleHelper.DisplayShopItemBuy(i+1, storeItem.Name, storeItem.StatType, storeItem.StatusBonus, storeItem.Comment, priceDisplay, _Width, _statWidth, _commentWidth, _priceWidth);
                }
                else
                {
                    // 번호 없이 표시
                    //Console.Write($"- {storeItem.Name,-18}");
                    ConsoleHelper.DisplayShopItem(storeItem.Name, storeItem.StatType, storeItem.StatusBonus, storeItem.Comment, priceDisplay, _Width, _statWidth, _commentWidth, _priceWidth);
                }

               // Console.Write($" | {storeItem.StatType,6} +{storeItem.StatusBonus,-3}");
               // Console.Write($" | {storeItem.Comment,-40}");
               // Console.WriteLine($" | {priceDisplay}");

                Console.ResetColor();
            }
        }

        // 아이템 판매 모드일 때 플레이어 인벤토리의 아이템을 출력하는 메서드 (플레이어 인벤토리의 아이템을 출력함)
        private void DisplayPlayerItemsForSale()
        {
            Player player = GameManager.Instance.Player;
            List<Item> playerItems = player.Inventory.Items;

            if (playerItems.Count == 0)
            {
                Info("  [판매할 아이템이 없습니다]");
                return;
            }

            for (int i = 0; i < playerItems.Count; i++)
            {
                Item item = playerItems[i];
                // 일단 임시로 구매가의 85%로 판매할 수 있다. (여기에서 판매가를 수정하시면 됩니다.)
                int sellPrice = (int)(item.Price * _storeDiscountRate);
                ConsoleHelper.DisplayShopItemSell(i+1, item.Name, item.StatType, item.StatusBonus, item.Comment, sellPrice, _Width, _statWidth, _commentWidth, _priceWidth);
                //Console.Write($"- {i + 1}. {item.Name,-15}");
                //Console.Write($" | {item.StatType} +{item.StatusBonus,-3}");
                //Console.Write($" | {item.Comment,-40}");
                //Console.WriteLine($" | {sellPrice} G");
            }
        }

        // ---------------------------- 여기부터 사용자 입력 받는 메서드입니다.
        // 상점 모드 이동 입력
        public void ItemDisplay()
        {
            string input = Console.ReadLine();
            switch (_currentMode)
            {
                case StoreMode.Main:
                    MainModeInput(input);
                    break;
                case StoreMode.Buying:
                    BuyingModeInput(input);
                    break;
                case StoreMode.Selling:
                    SellingModeInput(input);
                    break;
            }
        }


        // 현재 메인 모드 상태일 때 입력받는 값 처리하는 메서드
        private void MainModeInput(string input)
        {
            switch (input)
            {
                case "1":
                    _currentMode = StoreMode.Buying;
                    break;
                case "2":
                    _currentMode = StoreMode.Selling;
                    break;
                case "0":
                    GameManager.Instance.SwitchScene(GameState.TownScene);
                    _currentMode = StoreMode.Main;
                    break;
                default:
                    Info("잘못된 입력입니다.");
                    Thread.Sleep(900);
                    break;
            }
        }

        // 구매 모드에서 입력 처리
        private void BuyingModeInput(string input) 
        {
            if (int.TryParse(input, out int itemIndex))
            {
                if (itemIndex == 0)
                {
                    _currentMode = StoreMode.Main;
                    return;
                }

                List<Item> allItems = GameManager.Instance.AllItems;

                if (itemIndex > 0 && itemIndex <= allItems.Count)
                {
                    Item itemToBuy = allItems[itemIndex - 1]; //Id는 0부터 시작하니 -1
                    Player player = GameManager.Instance.Player;

                    if (player.Inventory.Items.Any(i => i.Id == itemToBuy.Id)) Info("이미 구매한 아이템입니다.");
                    else if (player.Gold < itemToBuy.Price) Info("골드가 부족합니다.");
                    else
                    {
                        player.AddGold(-itemToBuy.Price); //플레이어 골드 차감
                        player.Inventory.AddItem(itemToBuy.Clone());
                        Info($"{itemToBuy.Name}을(를) 구매했습니다!");
                    }
                    Thread.Sleep(900);
                }
                else
                {
                    Info("잘못된 번호입니다.");
                    Thread.Sleep(900);
                }
            }
            else
            {
                Info("잘못된 입력입니다.");
                Thread.Sleep(900);
            }
        }

        // 판매 모드에서 입력 처리
        private void SellingModeInput(string input) 
        {
            if (int.TryParse(input, out int itemIndex))
            {
                if(itemIndex == 0)
                {
                    _currentMode = StoreMode.Main;
                    return;
                }
                Player player = GameManager.Instance.Player;
                List<Item> playerItems = player.Inventory.Items;

                if (itemIndex > 0 && itemIndex <= playerItems.Count)
                {
                    Item itemToSell = playerItems[itemIndex - 1]; //Id는 0부터 시작하니 -1
                    int sellPrice = (int)(itemToSell.Price * _storeDiscountRate); //판매가격

                    if (player.Inventory.Items.Any(i => i.Id == itemToSell.Id))
                    {
                        player.AddGold(sellPrice); //플레이어 골드 증가
                        player.Inventory.RemoveItem(itemToSell);
                        Info($"{itemToSell.Name}을(를) {sellPrice} G 로 판매했습니다!");
                    }
                    Thread.Sleep(900);
                }
            }
        }
    }
}
