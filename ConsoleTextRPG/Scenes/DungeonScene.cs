using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Scenes
{
    public class DungeonScene : BaseScene
    {
        // 던전 클리어 조건
        int walkCount = 0; // 이동 횟수
        int dungeonClearCount = 15; // 던전 클리어 횟수
        double monsValue = 0.3f; // 몬스터 등장 확률 (30%)

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
                    DungeonEvent();
                    Thread.Sleep(500);
                    break;
                case 2:
                    Info("왼쪽길로 갑니다");
                    DungeonEvent();
                    Thread.Sleep(500);
                    break;
                case 3:
                    Console.WriteLine("\ninfo : 오른쪽길로 갑니다.");
                    DungeonEvent();
                    Thread.Sleep(500);
                    break;
                default:
                    Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                    Thread.Sleep(800);
                    break;
            }
        }

        void DungeonEvent()
        {
            if (walkCount < dungeonClearCount)
            {
                walkCount++; // 이동 횟수 증가
                SpawnMonster();
                return;
            }
            else
            {
                Console.WriteLine("\ninfo : 던전을 클리어했습니다.");
                Console.WriteLine("\ninfo : 마을로 돌아갑니다");
                GameManager.Instance.SwitchScene(GameState.TownScene); // 마을로 돌아가기
                Thread.Sleep(1000);
                walkCount = 0;// 이동 횟수 초기화
                return;
            }
        }

        // 몬스터 소환 로직
        private void SpawnMonster()
        {
            /*Random rand = new Random(); // 출현 몬스터 수 조정을 위한 랜덤 객체 생성
            int eventChance = rand.Next(1, 5); // 최소 1, 최대 4 마리 까지 생성하도록 설정*/

            Random rand1 = new Random();
            if (rand1.NextDouble() < monsValue) // 몬스터 등장 확률에 따라 몬스터 소환
            {
                Console.WriteLine("\ninfo : 몬스터가 나타났습니다!");
                Thread.Sleep(1000);
                //gameManager.SwitchScene(SceneID.배틀씬); // 배틀씬으로 전환
            }
            else
                return; // 몬스터가 등장하지 않음
        }
    }
}