using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Scene
{
    public class DungeonScene : BaseScene
    {
        // 던전 클리어 조건
        int walkCount = 0; // 이동 횟수

        public DungeonScene(GameManager game) : base(game)
        {
        }

        public override void Render()
        {
            Print("◎던전◎", ConsoleColor.Red);
            Print("3가지 선택지를 보고 길을 선택해주세요\n");
            Print("이동횟수 : ", walkCount, ConsoleColor.DarkGreen);
            Print("\n");
            Print(1, "왼쪽길", ConsoleColor.DarkCyan);
            Print(2, "앞으로", ConsoleColor.DarkCyan);
            Print(3, "오른쪽길", ConsoleColor.DarkCyan);

            Print("원하시는 행동을 입력해주세요");
            Console.Write(">>");
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
            switch (index)
            {
                case 1:
                    Info("앞으로 전진합니다");
                    Thread.Sleep(500);
                    break;
                case 2:
                    Info("왼쪽길로 갑니다");
                    Thread.Sleep(500);
                    break;
                case 3:
                    Console.WriteLine("\ninfo : 오른쪽길로 갑니다.");
                    Thread.Sleep(500);
                    break;
                default:
                    Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                    Thread.Sleep(800);
                    break;
            }
        }

    }
}