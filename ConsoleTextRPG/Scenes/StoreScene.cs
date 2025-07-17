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
                if (storeItem.Type == Item.ItemType.Potion)
                {
                    isSoldOut = false; // 물약은 판매하지 않으므로 항상 구매 가능
                }

                string priceDisplay = isSoldOut ? "구매완료" : $"{storeItem.Price} G";

                Console.ForegroundColor = isSoldOut ? ConsoleColor.DarkGray : ConsoleColor.White;

                if (showNumbers) // 구매 모드일 경우 아이템 앞에 번호를 출력한다.
                {
                    // 번호 표시 및 mode = 3 상점 구매창
                    ConsoleHelper.DisplayHelper(i + 1, storeItem.Name, myPlayer.Inventory.PotionCount, storeItem.StatType, storeItem.StatusBonus, storeItem.Comment, priceDisplay, storeItem.IsEquipped, i, 3);
                }
                else
                {
                    // 번호 없이 표시 및 mode = 1로 상점 전시창
                    ConsoleHelper.DisplayHelper(i + 1, storeItem.Name, myPlayer.Inventory.PotionCount, storeItem.StatType, storeItem.StatusBonus, storeItem.Comment, priceDisplay, storeItem.IsEquipped, i, 1);
                }

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
                // 판매가
                int sellPrice = (int)(item.Price * _storeDiscountRate);
                string priceSell = $"{sellPrice} G";
                // mode = 2로 판매창
                ConsoleHelper.DisplayHelper(i + 1, item.Name, myPlayer.Inventory.PotionCount, item.StatType, item.StatusBonus, item.Comment, priceSell, item.IsEquipped, i, 2);
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

                    if (player.Inventory.Items.Any(i => i.Id == itemToBuy.Id))
                    {
                        if (itemToBuy.Type == Item.ItemType.Potion) //물약은 중복 구매가능
                        {
                            player.Inventory.AddPotions(1);
                            Item newItem = itemToBuy.Clone();            

                            player.AddGold(-itemToBuy.Price);            
                            player.EquipItem(newItem);                   
                            player.Inventory.AddItem(newItem);           
                            Info($"{itemToBuy.Name}을(를) 구매했습니다!");
                            return;
                        }
                        else
                        {
                            Info("이미 구매한 아이템입니다.");
                        }
                    }
                    else if (player.Gold < itemToBuy.Price) 
                    { Info("골드가 부족합니다."); }
                    else
                    {
                        if(itemToBuy.Type == Item.ItemType.Potion) player.Inventory.AddPotions(1); //물약 구매시
                        Item newItem = itemToBuy.Clone();                                           //나중에 계정 저장 및 중복방지를 위한 아이템 클론 생성

                        player.AddGold(-itemToBuy.Price);                                           // 플레이어 골드 차감
                        player.EquipItem(newItem);                                                  // 구매시 바로 장착
                        player.Inventory.AddItem(newItem);                                          // 인벤토리에 아이템 추가
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
                List<Item> playerItems = myPlayer.Inventory.Items;

                if (itemIndex > 0 && itemIndex <= playerItems.Count)
                {
                    Item itemToSell = playerItems[itemIndex - 1]; //Id는 0부터 시작하니 -1
                    int sellPrice = (int)(itemToSell.Price * _storeDiscountRate); //판매가격

                    if (myPlayer.Inventory.Items.Any(i => i.Id == itemToSell.Id))
                    {
                        if (itemToSell.Type == Item.ItemType.Potion) myPlayer.Inventory.UsePotion();
                        myPlayer.AddGold(sellPrice);               // 플레이어 골드 증가
                        if (itemToSell.IsEquipped == true) 
                        { 
                            myPlayer.UnequipItem(itemToSell);       // 판매중 장착시 장착해제  
                        }
                        myPlayer.Inventory.RemoveItem(itemToSell);   // 인벤토리 아이템 제거
                        Info($"{itemToSell.Name}을(를) {sellPrice} G 로 판매했습니다!");
                    }
                    Thread.Sleep(900);
                }
            }
        }

    }
}
