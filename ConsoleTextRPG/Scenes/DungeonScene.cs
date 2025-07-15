using ConsoleTextRPG.Data;
using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        bool isBattle = false; // 배틀 여부
        public override void Render()
        {
            if( isBattle)
            {
                BattleRender(); // 배틀씬 랜더링
                return;
            }
            DungeonRender();
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
            if (isBattle) // 배틀씬일 경우
            {
                BattleMove(index); // 배틀 행동 선택
                return;
            }
            // 던전 씬일 경우
            DungeonMove(index); // 던전 행동 선택
        }


        // 던전 씬 랜더함수
        void DungeonRender()
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
        // 배틀씬 랜더함수
        public void BattleRender()
        {
            Print("◎Battle!!◎", ConsoleColor.DarkYellow);
            // 몬스터 리스트 전개(list와 랜덤으로 몬스터 마리수 조정)
            for (int i = 0; i < Monster.monsterlist.Count; i++)
            {
                Print(i+1,$"| {Monster.monsterlist[i].Name}  |  {Monster.monsterlist[i]} \n");
            }
            Print("몬스터와의 전투가 시작되었습니다!\n");
            Print(1, "공격", ConsoleColor.DarkCyan);
            Print(2, "방어", ConsoleColor.DarkCyan);
            Print(3, "종료", ConsoleColor.DarkCyan);
            Print("원하시는 행동을 입력해주세요");
            Console.Write(">>");
        }


        void DungeonMove(int index)
        {
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

        void BattleMove(int index)
        {
            switch (index)
            {
                case 1:
                    Info("공격합니다");
                    // FSM 공격 상태로 전환 로직 추가
                    Thread.Sleep(500);
                    break;
                case 2:
                    Info("방어합니다");
                    Thread.Sleep(500);
                    break;
                case 3:
                    Info("전투를 종료합니다");
                    isBattle = false; // 배틀 종료
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
                Monster.Init(); // 몬스터 목록 초기화
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
                isBattle= true; // 배틀 시작
                Console.WriteLine("\ninfo : 몬스터가 나타났습니다!");
                Thread.Sleep(1000);
                //gameManager.SwitchScene(SceneID.배틀씬); // 배틀씬으로 전환
            }
            else
                return; // 몬스터가 등장하지 않음
        }
    }
}

// FSM의 상태들
namespace ConsoleTextRPG.TurnBasedSystem
{
    public enum State
    {
        Idle,
        Battle,
        EndBattle,
        Size,// Size는 현재 배열의 크기를 시각적으로 나타내주기위한 요소임
    }

    public class IdelState : BaseState
    {
        public override void Enter()
        {
        }
        public override void Update()
        {
        }
        public override void Exit()
        {
        }
    }

    public class BattleState : BaseState
    {
        public override void Enter()
        {
           
        }
        public override void Update()
        {
         
        }
        public override void Exit()
        {
          
        }
    }

    public class EndBattleState : BaseState
    {
        public override void Enter()
        {
           
        }
        public override void Update()
        {
          
        }
        public override void Exit()
        {
            GameManager.monsterlist.Clear(); // 몬스터 목록 초기화

        }
    }
}



