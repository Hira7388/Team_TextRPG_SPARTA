using ConsoleTextRPG.Data;
using ConsoleTextRPG.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Scenes
{
    public class InventoryScene : BaseScene
    {
        public override void Render()
        {
            Print("◎인벤토리◎", ConsoleColor.Red);
            Print("보유 중인 아이템을 관리할 수 있습니다.\n");
            Print("\n");
            Print("[아이템 목록]\n");

            // 보유중인 아이템 목록 보여주는 메서드

            Print("원하시는 행동을 입력해주세요");
            Console.Write(">> ");
        }

        public override void Update()
        {
            string input = Console.ReadLine();
            int index;
            if (!int.TryParse(input, out index))
            {
                Info("잘못된 입력입니다.");
                Thread.Sleep(800);
                return;
            }

        }

        public void ShowInventoryItem()
        {
            foreach(Item item in Player.Inventory.Items)
            Console.WriteLine(
        }
    }
}
