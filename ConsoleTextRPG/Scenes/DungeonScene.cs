using ConsoleTextRPG.Data;
using ConsoleTextRPG.Managers;
using ConsoleTextRPG.Monsters;
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
        bool battleInitialized = false;                 // 수정: 전투 초기화 플래그 추가

        List<Monster> currentMonsters = new();

        public override void Render()
        {
            Console.Clear(); // 콘솔 화면 초기화
            if(isBattle)
                BattleRender(); // 배틀씬 랜더링
            else
                DungeonRender(); // 던전 씬 랜더링
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
            if (isBattle)
                BattleMove(index); // 배틀 행동 선택
            else
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

            Print("\n원하시는 행동을 입력해주세요");
            Console.Write(">>");
        }

        // 몬스터 등장 함수
        // 몬스터 소환 로직
        void BattleRender()
        {
                isBattle = true; // 배틀 시작
                Print("◎Battle!!◎", ConsoleColor.DarkYellow);
                Print($"\n몬스터가 {currentMonsters.Count}마리가 나타났습니다!\n");
                Print("\n============[몬스터]============");
                for (int i = 0; i < currentMonsters.Count; i++)
                {
                    Console.WriteLine($"{currentMonsters[i].PrintMonster(i+1)}");
                }

                Print("\n===========[전투선택지]===========");
                Print(1, "공격", ConsoleColor.DarkCyan);
                Print(2, "방어", ConsoleColor.DarkCyan);
                Print(3, "종료", ConsoleColor.DarkCyan);
                Print("\n원하시는 행동을 입력해주세요");
                Console.Write(">>");
        }

        void InitBattle()
        {
            if (battleInitialized) return; // 이미 배틀이 초기화된 경우, 중복 초기화를 방지
            battleInitialized = true;
            isBattle = true;
            currentMonsters.Clear(); // 몬스터 목록 초기화

            var rnd = new Random();
            var types = GameManager.Instance.monsType.Keys.ToList(); // 몬스터 타입 목록 가져오기
            int MonsterCount = new Random().Next(1, 4); // 최소 1, 최대 3 마리 까지 생성하도록 설정

            for (int i = 0; i < MonsterCount; i++)
            {
                var mType = types[rnd.Next(types.Count)];

                switch(mType)
                {
                    case MonsterType.Minion:
                        currentMonsters.Add(new Minion());
                        break;
                    case MonsterType.SigeMinion:
                        currentMonsters.Add(new SiegeMinion());
                        break;
                    case MonsterType.Voidgrub:
                        currentMonsters.Add(new Voidgrub());
                        break;
                }
            }
        }


        void DungeonEvent()
        {
            if (walkCount < dungeonClearCount)
            {
                walkCount++; // 이동 횟수 증가
                if (new Random().NextDouble() < monsValue)  // 몬스터 등장 확률 체크
                    InitBattle();
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

        void DungeonMove(int index)
        {
            switch (index)
            {
                case 1:
                    Info("왼쪽길로 갑니다");
                    DungeonEvent();
                    Thread.Sleep(200);
                    break;
                case 2:
                    Info("앞으로 갑니다");
                    DungeonEvent();
                    Thread.Sleep(200);
                    break;
                case 3:
                    Console.WriteLine("\ninfo : 오른쪽길로 갑니다.");
                    DungeonEvent();
                    Thread.Sleep(200);
                    break;
                default:
                    Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                    Thread.Sleep(300);
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
                    Thread.Sleep(200);
                    break;
                case 2:
                    Info("방어합니다");
                    Thread.Sleep(200);
                    break;
                case 3:
                    Info("전투를 종료합니다");
                    isBattle = false; // 배틀 종료
                    Thread.Sleep(200);
                    break;
                default:
                    Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                    Thread.Sleep(300);
                    break;
            }
        }
    }
}

// FSM의 상태들
namespace ConsoleTextRPG.TurnBasedSystem
{


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

        }
    }
}



