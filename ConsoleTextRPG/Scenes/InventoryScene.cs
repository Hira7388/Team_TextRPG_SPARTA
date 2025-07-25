﻿using ConsoleTextRPG.Data;
using ConsoleTextRPG.Managers;
using ConsoleTextRPG.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Scenes
{
    public class InventoryScene : BaseScene
    {
        List<Item> items = GameManager.Instance.Player.Inventory.Items;

        public override void RenderMenu()
        {
            ShowInventoryMenu();
        }


        public override void UpdateInput()
        {
            InventoryInput();
        }

        Player myPlayer = GameManager.Instance.Player;

        private void ShowInventoryMenu()
        {
            Print("◎인벤토리◎", ConsoleColor.Red);
            Print("보유 중인 아이템을 관리할 수 있습니다.");
            Print("");
            Print("[아이템 목록]");

            ShowInventoryItem();

            Print("");
            Print("장착 또는 사용할 아이템의 번호를 입력해주세요. (1~9)"); 
            Print("0. 나가기");
            Print("");
            Print("원하시는 행동을 입력해주세요");
            Console.Write(">> ");
        }
        private void InventoryInput()
        {
            string input = Console.ReadLine();
            int index;
            if (int.TryParse(input, out index))
            {
                if(index == 0)
                {
                    Print("[타운으로 향합니다.]");
                    Thread.Sleep(500);
                    GameManager.Instance.SwitchScene(GameState.TownScene);
                }
                else if(index > 0 && index <=items.Count) //인벤토리와 입력한 숫자를 비교해서 아이템을 장착하거나 해제합니다.
                {
                    int itemIndex = index - 1;
                    Item targetItem = items[itemIndex];
                    
                    // 회복약 사용로직 추가
                    if(items[itemIndex].Type == Item.ItemType.Potion)
                    {
                        int beforeHp = myPlayer.Stat.CurrentHp;
                        Print($"[ {targetItem.Name} ] 을(를) 선택했습니다.");
                        Print($"체력을 {targetItem.StatusBonus} 만큼 회복합니다.");
                        myPlayer.EatPotion(targetItem);
                        myPlayer.Inventory.UsePotion();
                        Print($"[HP] {beforeHp} -> {myPlayer.Stat.CurrentHp}");

                        Thread.Sleep(800);
                        return;
                    }
                    //아이템 장착/해제 로직
                    if (targetItem.IsEquipped)
                    {
                        targetItem.IsEquipped = false;
                        myPlayer.UnequipItem(targetItem);
                        Print($"[ {targetItem.Name} ] 을(를) 해제했습니다.");
                        Thread.Sleep(800);
                    }
                    else
                    {
                        targetItem.IsEquipped = true;
                        myPlayer.EquipItem(targetItem);
                        Print($"[ {targetItem.Name} ] 을(를) 장착했습니다.");
                        Thread.Sleep(800);
                    }
                }
                else
                {
                    Print("인벤토리가 비어있습니다. [타운으로 향합니다.]");
                    Thread.Sleep(800);
                    GameManager.Instance.SwitchScene(GameState.TownScene);
                }
            }
            else
            {
                Info("잘못된 입력입니다.(인벤토리씬)");
                Thread.Sleep(800);
                return;
            }

        }

        public void ShowInventoryItem()
        {

            // 우선 보유중인 아이템이 있는지 확인
            if (items.Count == 0)
            {
                Print("  [비어 있음]", ConsoleColor.DarkGray);
                return;
            }

            // 아이템 목록을 번호와 함께 출력합니다.
            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];

                // 아이템 능력치와 설명 출력 mode = 0으로 인벤토리 창
                ConsoleHelper.DisplayHelper(i + 1, item.Name, myPlayer.Inventory.PotionCount, item.StatType, item.StatusBonus, item.Comment, "0", item.IsEquipped, (int)item.Type, 0);
            }

            Print("");
        }

    }
}
