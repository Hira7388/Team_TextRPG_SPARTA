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

        double monsValue = 0.5f; // 몬스터 등장 확률(ex. 0.5f = 50% 확률로 몬스터 등장)

        bool battleInitialized = false;                 // 수정: 전투 초기화 플래그 추가
        Player myPlayer = GameManager.Instance.Player; // 플레이어 객체 가져오기

        List<Monster> currentMonsters = new();

        public override void RenderMenu()
        {
            Console.Clear(); // 콘솔 화면 초기화
            switch(GameManager.Instance.currentState)
            {
                case DungeonState.Idle:
                    DungeonRender(); // 던전 씬 랜더링
                    break;
                case DungeonState.PlayerTrun:
                    PlayerTurnRender();
                    break;
                case DungeonState.PlayerAttack:
                    PlayerAttackRender();
                    break;
                case DungeonState.EnemyTurn:
                    //
                    break;
                case DungeonState.EndBattle:
                    //
                    break;
            }
        }

        public override void UpdateInput()
        {
            string input = Console.ReadLine();
            int index;
            if (!int.TryParse(input, out index))
            {
                Info("잘못된 입력입니다.");
                Thread.Sleep(800);
                return;
            }
            switch (GameManager.Instance.currentState)
            {
                case DungeonState.Idle:
                    DungeonMove(index); // 던전 행동 선택
                    break;
                case DungeonState.PlayerTrun:
                    PlayerTrunMove(index); // 플레이어 턴 행동 선택
                    break;
                case DungeonState.PlayerAttack:
                    PlayerAttackMove(index); // 플레이어 공격 행동 선택
                    break;
                case DungeonState.EnemyTurn:
                    //
                    break;
                case DungeonState.EndBattle:
                    //
                    break;
            }                
        }

        // ==============[던전상태]==============
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
                    Info("오른쪽길로 갑니다");
                    DungeonEvent();
                    Thread.Sleep(200);
                    break;
                default:
                    Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                    Thread.Sleep(300);
                    break;
            }
        }

        void DungeonEvent()
        {
            if (walkCount < dungeonClearCount)
            {
                walkCount++; // 이동 횟수 증가
                if (new Random().NextDouble() < monsValue)  // 몬스터 등장 확률 체크
                {
                    GameManager.Instance.currentState = DungeonState.PlayerTrun;
                    SwapnMonster();
                }
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

        void SwapnMonster()
        {
            if (battleInitialized) return; // 이미 배틀이 초기화된 경우, 중복 초기화를 방지
            battleInitialized = true;
            currentMonsters.Clear(); // 몬스터 목록 초기화

            var rnd = new Random();
            var types = GameManager.Instance.monsType.Keys.ToList(); // 몬스터 타입 목록 가져오기
            int MonsterCount = new Random().Next(1, 4); // 최소 1, 최대 3 마리 까지 생성하도록 설정

            for (int i = 0; i < MonsterCount; i++)
            {
                var mType = types[rnd.Next(types.Count)];

                switch (mType)
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

        // ==============[플레이어턴상태]==============
        void PlayerTurnRender()
        {
            Print("◎Battle!!◎", ConsoleColor.DarkYellow);
            Print($"\n몬스터가 {currentMonsters.Count}마리가 나타났습니다!\n");
            Print("\n============[몬스터]============");
            for (int i = 0; i < currentMonsters.Count; i++)
            {
                currentMonsters[i].PrintMonster();
            }

            Print("===========[선택지]===========");
            Print(1, "공격", ConsoleColor.DarkCyan);
            Print(2, "방어", ConsoleColor.DarkCyan);
            Print(3, "도망", ConsoleColor.DarkCyan);

            Print("\n원하시는 행동을 입력해주세요");
            Console.Write(">>");
        }

        void PlayerTrunMove(int index)
        {
            switch (index)
            {
                case 1:
                    GameManager.Instance.currentState = DungeonState.PlayerAttack;
                    break;
                case 2:
                    PlayerDefend(index - 1);
                    break;
                case 3:
                    PlayerRun();
                    break;
                default:
                    Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                    Thread.Sleep(300);
                    break;
            }
        }

        void PlayerDefend(int i)
        {
            Info("방어합니다");
            myPlayer.Defend(currentMonsters[i]);
            Thread.Sleep(200);
            GameManager.Instance.currentState = DungeonState.EnemyTurn;
        }
        void PlayerRun()
        {
            if(currentMonsters.Count > 1)
            {
                if (new Random().NextDouble() < 0.3f) // 30% 확률로 도망 성공
                {
                    Info("도망쳤습니다");
                    Thread.Sleep(200);
                    GameManager.Instance.currentState = DungeonState.Idle;
                    return;
                }
                else
                {
                    Info("도망치지 못했습니다.");
                    Thread.Sleep(200);
                    GameManager.Instance.currentState = DungeonState.EnemyTurn;
                    return;
                }
            }
            Info("도망쳤습니다.");
            Thread.Sleep(200);
            GameManager.Instance.currentState = DungeonState.Idle;
        }

        // ==============[플레이어 공격 상태]==============

        void PlayerAttackRender()
        {
            Print("◎Battle!!◎", ConsoleColor.DarkYellow);
            Print($"\n공격할 대상을 선택해주세요.\n");
            Print("\n============[몬스터]============");
            for (int i = 0; i < currentMonsters.Count; i++)
            {
                currentMonsters[i].PrintMonster();
            }

            Print("===========[선택지]===========");
            Print(0, "취소", ConsoleColor.DarkCyan);

            Print("\n원하시는 행동을 입력해주세요");
            Console.Write(">>");
        }

        void PlayerAttackMove(int index)
        {
            switch (index)
            {
                case 1:
                    PlayerAttack(index - 1);
                    break;
                case 2:
                    PlayerAttack(index - 1);
                    break;
                case 3:
                    PlayerAttack(index - 1);
                    break;
                case 0:
                    GameManager.Instance.currentState = DungeonState.PlayerTrun;
                    break;
                default:
                    Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                    Thread.Sleep(300);
                    break;
            }
        }

        void PlayerAttack(int i)
        {
            Info("공격합니다");
            myPlayer.Attack(currentMonsters[i]);
            Thread.Sleep(200);
            GameManager.Instance.currentState = DungeonState.EnemyTurn;
        }

        // ==============[몬스터턴상태]==============
        void EnemyTurnRender()
        {
            Print("◎Battle!!◎", ConsoleColor.DarkYellow);
            Print($"\n몬스터가 {currentMonsters.Count}마리가 나타났습니다!\n");
            Print("\n============[몬스터]============");
            for (int i = 0; i < currentMonsters.Count; i++)
            {
                currentMonsters[i].PrintMonster(i + 1, ConsoleColor.Green);
            }

            Print("===========[대상선택지]===========");
            Print(1, "공격", ConsoleColor.DarkCyan);
            Print(2, "방어", ConsoleColor.DarkCyan);
            Print(3, "종료", ConsoleColor.DarkCyan);

            Print("\n원하시는 행동을 입력해주세요");
            Console.Write(">>");
        }



    }
}



