using ConsoleTextRPG.Data;
using ConsoleTextRPG.Managers;
using ConsoleTextRPG.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Scenes
{
    public class InventoryScene : BaseScene
    {   
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
            //Print("◎인벤토리◎", ConsoleColor.Red);
            //Print("보유 중인 아이템을 관리할 수 있습니다.\n");
            //Print("\n");
            //Print("[아이템 목록]\n");
            Console.WriteLine("◎인벤토리◎");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("\n");
            Console.WriteLine("[아이템 목록]\n");
            ShowInventoryItem();

            //Print("원하시는 행동을 입력해주세요");
            Console.WriteLine("원하시는 행동을 입력해주세요");
            Console.Write(">> ");
        }
        private void InventoryInput()
        {
            string input = Console.ReadLine();
            int index;
            if (!int.TryParse(input, out index))
            {
                Info("잘못된 입력입니다.(인벤토리씬)");
                Thread.Sleep(800);
                return;
            }

        }

        public void ShowInventoryItem()
        {
            List<Item> items = myPlayer.Inventory.Items;

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

                Console.Write("- ");

                // 장착 여부에 따라 '[E]' 표시 및 색상 변경
                if (item.IsEquipped)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("[E]");
                    Console.ResetColor();
                    Console.Write($" {item.Name,-15}");
                }
                else
                {
                    Console.Write($"   {item.Name,-15}");
                }

                // 아이템 능력치와 설명 출력
                Console.Write($" | {item.StatType} +{item.StatusBonus,-3}");
                Console.WriteLine($" | {item.Comment}");
            }
        }
    }
}
