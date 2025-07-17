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
        int dungeonClearCount = 0; // 던전 클리어 횟수
        int deadCount = 0; // 죽은 몬스터 수
        int dungeonHP = 0; // 결과창 확인용

        bool isAtive = false; // 던전 활성화 여부

        bool isDF = false; // 방어 여부
        bool isWin = false; // 승리 여부

        double monsValue = 0.5f; // 몬스터 등장 확률(ex. 0.5f = 50% 확률로 몬스터 등장)

        Player myPlayer = GameManager.Instance.Player; // 플레이어 객체 가져오기

        Queue<Monster> monsterQueue = new Queue<Monster>();   // 몬스터 공격 순서를 저장하는 큐
        List<Monster> currentMonsters= new List<Monster>();

        public override void RenderMenu()
        {
            switch(GameManager.Instance.currentState)
            {
                case DungeonState.SelectStart:
                    SelectStartRender();
                    break;
                case DungeonState.SelectLevel:
                    SelectLevelRender(); 
                    break;
                case DungeonState.Adventure:
                    DungeonRender();
                    break;
                case DungeonState.PlayerTrun:
                    PlayerTurnRender();
                    break;
                case DungeonState.PlayerAttack:
                    PlayerAttackRender();
                    break;
                case DungeonState.EnemyTurn:
                    EnemyTurnRender();
                    break;
                case DungeonState.EndBattle:
                    EndBattleRender();
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
                case DungeonState.SelectStart:
                    SelectStartMove(index);
                    break;
                case DungeonState.SelectLevel:
                    SelectLevelMove(index);
                    break;
                case DungeonState.Adventure:
                    DungeonMove(index); // 던전 행동 선택
                    break;
                case DungeonState.PlayerTrun:
                    PlayerTrunMove(index); // 플레이어 턴 행동 선택
                    break;
                case DungeonState.PlayerAttack:
                    PlayerAttackMove(index); // 플레이어 공격 행동 선택
                    break;
                case DungeonState.EnemyTurn:
                    EnemyAttackMove(index); // 몬스터 턴 행동 선택
                    break;
                case DungeonState.EndBattle:
                    EndTrunMove(index); // 전투 종료 행동 선택
                    break;
            }                
        }

        // ============================[던전선택]============================
        void SelectStartRender()
        {
         
            Print("◎던전◎", ConsoleColor.DarkYellow);
            Print("던전 진행을 선택해주세요\n");
            if (isAtive)
            {
                int num = (walkCount *100) / dungeonClearCount;
                Print(1, $"계속하기 | 진행도({num}%)", ConsoleColor.DarkCyan);
                Print(2, "새로하기", ConsoleColor.DarkCyan);
            }
            else
            Print(1, "새로하기", ConsoleColor.DarkCyan);
            Print(0, "마을로 돌아가기", ConsoleColor.DarkCyan);

            Print("\n원하시는 행동을 입력해주세요");
            Console.Write(">>");
        }
        void SelectStartMove(int index)
        {
            if (isAtive)
            {
                switch (index)
                {
                    case 1:
                        Info("계속 진행합니다");
                        GameManager.Instance.currentState = DungeonState.Adventure;
                        Thread.Sleep(100);
                        break;
                    case 2:
                        Info("던전 처음부터 진행합니다");
                        isAtive = false;
                        GameManager.Instance.currentLevel = DungeonLevel.Normal;
                        GameManager.Instance.currentState = DungeonState.SelectLevel;
                        Thread.Sleep(100);
                        break;
                    case 0:
                        Info("마을로 돌아갑니다");
                        GameManager.Instance.SwitchScene(GameState.TownScene); // 마을로 돌아가기
                        Thread.Sleep(100);
                        break;
                    default:
                        Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                        Thread.Sleep(200);
                        break;
                }
            }
            else
            {
                switch (index)
                {
                    case 1:
                        Info("던전 처음부터 진행합니다");
                        walkCount = 0; // 이동 횟수 초기화
                        isAtive = false;
                        GameManager.Instance.currentLevel = DungeonLevel.Normal;
                        GameManager.Instance.currentState = DungeonState.SelectLevel;
                        Thread.Sleep(100);
                        break;
                    case 0:
                        Info("마을로 돌아갑니다");
                        GameManager.Instance.SwitchScene(GameState.TownScene); // 마을로 돌아가기
                        Thread.Sleep(100);
                        break;
                    default:
                        Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                        Thread.Sleep(200);
                        break;
                }
                 
                }
        }

        // ============================[던전선택]============================
        void SelectLevelRender()
        {
            Print("◎던전-난이도 선택◎", ConsoleColor.DarkYellow);
            Print("던전의 난이도를 선택해주세요.\n");

            Print(1, "쉬움 난이도 | 몬스터 최대 2마리 까지만 출현", ConsoleColor.DarkCyan);
            Print(2, "보통 난이도 | 몬스터 최대 3마리 까지만 출현", ConsoleColor.DarkCyan);
            Print(3, "어려움난이도 | 몬스터 최대 4마리 까지만 출현", ConsoleColor.DarkCyan);
            Print(0, "마을로 돌아가기", ConsoleColor.DarkCyan);

            Print("\n원하시는 행동을 입력해주세요");
            Console.Write(">>");
        }
        void SelectLevelMove(int index)
        {
            switch (index)
            {
                case 1:
                    Info("쉬움난이도로 진행합니다.");
                    Info("전투보다 스토리를 더 중요시 여기는 분들에게 알맞는 난이도 입니다.");
                    dungeonClearCount = 10;
                    GameManager.Instance.currentLevel = DungeonLevel.Easy;
                    GameManager.Instance.currentState = DungeonState.Adventure;
                    Thread.Sleep(200);
                    break;
                case 2:
                    Info("보통 난이도로 진행합니다.");
                    dungeonClearCount = 15;
                    GameManager.Instance.currentLevel = DungeonLevel.Normal;
                    GameManager.Instance.currentState = DungeonState.Adventure;
                    Thread.Sleep(100);
                    break;
                case 3:
                    Info("어려움 난이도로 진행합니다.");
                    dungeonClearCount = 20;
                    GameManager.Instance.currentLevel = DungeonLevel.Hard;
                    GameManager.Instance.currentState = DungeonState.Adventure;
                    Thread.Sleep(100);
                    break;
                    case 0:
                        Info("마을로 돌아갑니다");
                        GameManager.Instance.SwitchScene(GameState.TownScene); // 마을로 돌아가기
                        Thread.Sleep(100);
                        break;
                default:
                    Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                    Thread.Sleep(200);
                    break;
            }
        }

        // ============================[던전상태]============================
        // 던전 씬 랜더함수
        void DungeonRender()
        {
            deadCount = 0; // 죽은 몬스터 수 초기화
            Print("◎던전◎", ConsoleColor.Red);
            Print("3가지 선택지를 보고 길을 선택해주세요\n");
            Print("이동횟수 : ", walkCount, ConsoleColor.DarkGreen);
            Print("\n");
            Print(1, "왼쪽길", ConsoleColor.DarkCyan);
            Print(2, "앞으로", ConsoleColor.DarkCyan);
            Print(3, "오른쪽길", ConsoleColor.DarkCyan);
            Print(0, "마을로 돌아가기", ConsoleColor.DarkCyan);

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
                    Thread.Sleep(100);
                    break;
                case 2:
                    Info("앞으로 갑니다");
                    DungeonEvent();
                    Thread.Sleep(100);
                    break;
                case 3:
                    Info("오른쪽길로 갑니다");
                    DungeonEvent();
                    Thread.Sleep(100);
                    break;
                case 0:
                    Info("마을로 돌아갑니다");
                    GameManager.Instance.currentState = DungeonState.SelectStart;
                    GameManager.Instance.SwitchScene(GameState.TownScene); // 마을로 돌아가기
                    isAtive = true;
                    Thread.Sleep(100);
                    break;
                default:
                    Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                    Thread.Sleep(200);
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
                walkCount = 0;// 이동 횟수 초기화
                Thread.Sleep(1000);
                return;
            }
        }

        void SwapnMonster()
        {
            // 난이도별 몬스터수 조정
            int max = 0;
            int min = 1;
            switch (GameManager.Instance.currentLevel)
            {
                case DungeonLevel.Easy:
                    max = 3;
                    break;
                case DungeonLevel.Normal:
                    max = 4;
                    break;
                case DungeonLevel.Hard:
                    min = 2; // 최소 2마리
                    max = 5;
                    break;
            }
            currentMonsters.Clear(); // 몬스터 목록 초기화
            var rnd = new Random();
            var types = GameManager.Instance.monsType.Keys.ToList(); // 몬스터 타입 목록 가져오기
            int MonsterCount = new Random().Next(min, max); // 최소 1, 최대 3 마리 까지 생성하도록 설정

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

        // ============================[플레이어턴상태]============================
        void PlayerTurnRender()
        {
            dungeonHP = myPlayer.Stat.CurrentHp; // 현재 플레이어 체력 저장
            Print("◎Battle!!◎", ConsoleColor.DarkYellow);
            Print($"\n몬스터가 {currentMonsters.Count}마리가 나타났습니다!\n");
            Print("\n============[몬스터]============");

            foreach(var monster in currentMonsters)
            {
                if (monster.Stat.IsDead)
                    monster.PrintMonster(ConsoleColor.DarkGray);
                else
                    monster.PrintMonster();
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
                    Info("방어합니다");
                    PlayerDefend();
                    break;
                case 3:
                    PlayerRun();
                    break;
                default:
                    Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                    // 입력 버퍼 비우기
                    while (Console.KeyAvailable) Console.ReadKey(true);

                    Thread.Sleep(300);

                    // 또 한 번 비워주기 (남아있을 수도 있으니까)
                    while (Console.KeyAvailable) Console.ReadKey(true); break;
            }
        }

        void PlayerDefend()
        {
            isDF= true; // 방어 상태로 변경
            GameManager.Instance.currentState = DungeonState.EnemyTurn;
            MonstersDeadCheck();
        }

        void PlayerRun()
        {
            if (currentMonsters.Count > 1)
            {
                if (new Random().NextDouble() < 0.1f) // 10% 확률로 도망 성공
                {
                    if (new Random().NextDouble() < 0.3f) // 30% 확률로 돈 흘림
                        LoseGold();

                    GameManager.Instance.currentState = DungeonState.Adventure;
                    Info("도망쳤습니다");
                    Thread.Sleep(500);
                    return;
                }
                else
                {
                    Info("도망치지 못했습니다.");
                    GameManager.Instance.currentState = DungeonState.EnemyTurn;
                    MonstersDeadCheck();
                    return;
                }
            }
            else
            {
                if (new Random().NextDouble() < 0.3f)
                LoseGold();
                Info("도망쳤습니다.");
                GameManager.Instance.currentState = DungeonState.Adventure;
                Thread.Sleep(200);
            }
            // 입력 버퍼 비우기
            while (Console.KeyAvailable) Console.ReadKey(true);

            Thread.Sleep(500);

            // 또 한 번 비워주기 (남아있을 수도 있으니까)
            while (Console.KeyAvailable) Console.ReadKey(true);

        }
        void LoseGold()
        {
            Info("도망치다가 소지금을 흘렸습니다");
            myPlayer.Gold -= (int)(myPlayer.Gold * 0.05f); // 소지금 5% 감소
        }



        // ============================[플레이어 공격 상태]============================
        void PlayerAttackRender()
        {
            Print("◎Battle!!◎", ConsoleColor.DarkYellow);
            Print($"\n공격할 대상을 선택해주세요.\n");
            Print("\n============[몬스터]============");

            for (int i = 0; i < currentMonsters.Count; i++)
            {
                currentMonsters[i].PrintMonster(i + 1, ConsoleColor.DarkCyan, ConsoleColor.DarkGray);
            }

            Print(" ");
            Print(0, "공격취소", ConsoleColor.DarkCyan);

            Print("\n원하시는 몬스터의 번호를 입력해주세요");
            Console.Write(">>");
        }

        void PlayerAttackMove(int index)
        {
            if(index < 0 || index > currentMonsters.Count)
            {
                Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                // 입력 버퍼 비우기
                while (Console.KeyAvailable) Console.ReadKey(true);

                Thread.Sleep(300);

                // 또 한 번 비워주기 (남아있을 수도 있으니까)
                while (Console.KeyAvailable) Console.ReadKey(true); return;
            }

            if( index == 0)
            {
                GameManager.Instance.currentState = DungeonState.PlayerTrun; // 공격 취소시 플레이어 턴으로 돌아감
                Info("공격을 취소합니다.");
                // 입력 버퍼 비우기
                while (Console.KeyAvailable) Console.ReadKey(true);

                Thread.Sleep(300);

                // 또 한 번 비워주기 (남아있을 수도 있으니까)
                while (Console.KeyAvailable) Console.ReadKey(true); return;
            }
            else
            {
                if (currentMonsters[index-1].Stat.CurrentHp <= 0)
                {
                    Print("\ninfo : 이미 죽은 몬스터입니다.");
                    // 입력 버퍼 비우기
                    while (Console.KeyAvailable) Console.ReadKey(true);

                    Thread.Sleep(300);

                    // 또 한 번 비워주기 (남아있을 수도 있으니까)
                    while (Console.KeyAvailable) Console.ReadKey(true); return;
                }
                else
                {
                    PlayerAttack(index - 1);
                }
            }
        }

        void PlayerAttack(int index)
        {
            // ---- [정진규 추가]
            bool wasAlive = !currentMonsters[index].Stat.IsDead; // 현재 공격하려는 몬스터가 살아있는지 확인한다.

            // 중독상태 같은 상태이상 공격이 있을 경우는 플레이어 공격전에 DeadCheck를 먼저 실행해야함
            myPlayer.Attack(currentMonsters[index]);

            // ---- [정진규 추가]
            bool isDeadNow = currentMonsters[index].Stat.IsDead; // 이번 공격으로 몬스터가 죽었나?
            if(wasAlive && isDeadNow)
            {
                QuestManager.Instance.OnMonsterKilled(currentMonsters[index].Name);
            }

            MonstersDeadCheck();
        }

        void MonstersDeadCheck()
        {
            deadCount = 0;
            // 다음 몬스터턴 행동에 사용될 살아있는 몬스터들 큐에 추가
            monsterQueue.Clear();
            foreach (var monster in currentMonsters)
            {
                if (!monster.Stat.IsDead)               //  살아있는 몬스터 필터링
                {
                    monsterQueue.Enqueue(monster);
                }
            }

            // 몬스터들 전부 죽었는지  확인
            foreach (var monster in currentMonsters)
            {
                if (monster.Stat.IsDead)
                    deadCount++;

                if (deadCount == currentMonsters.Count)
                {
                    isWin = true; // 모든 몬스터를 처치했을 때 승리 상태로 변경
                    GameManager.Instance.currentState = DungeonState.EndBattle;
                    Info("모든 몬스터를 처치했습니다.");
                }
                else
                {
                    GameManager.Instance.currentState = DungeonState.EnemyTurn;
                }



            }
                            Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey(true); // true: 입력된 키를 콘솔에 표시하지 않음

        }

        // ==============[몬스터턴상태]==============// 여기부터 구현하면댐
        void EnemyTurnRender()
        {
            Print("◎Battle!!◎", ConsoleColor.DarkYellow);
            Print($"\n몬스터의 공격이 시작됩니다!\n");
            Print("\n============[몬스터]============");
            foreach (var monster in currentMonsters)
            {
                if (monster.isDead)
                    monster.PrintMonster(ConsoleColor.DarkGray);
                else
                    monster.PrintMonster();
            }
            Print("===========[대상선택지]===========");
            Print(0, "다음", ConsoleColor.DarkCyan);

            Print("\n원하시는 행동을 입력해주세요");
            Console.Write(">>");
        }

        void EnemyAttackMove(int index)
        {
            if (index < 0 || index > currentMonsters.Count)
            {
                Print("\ninfo : 잘못 입력 하셨습니다.");
                // 입력 버퍼 비우기
                while (Console.KeyAvailable) Console.ReadKey(true);

                Thread.Sleep(200);

                // 또 한 번 비워주기 (남아있을 수도 있으니까)
                while (Console.KeyAvailable) Console.ReadKey(true); return;
            }

            if( index == 0)
            {            
                if (monsterQueue.Count > 0)
                {
                    var nextMonster = monsterQueue.Dequeue(); // 큐에서 몬스터를 하나씩 꺼내서 공격
                    int idx = currentMonsters.IndexOf(nextMonster); //list.IndexOf : 리스트속 <T> 객체의 인덱스를 반환
                    EnemyAttack(idx);
                }

                else
                {
                    dungeonHP = myPlayer.Stat.CurrentHp; // 현재 플레이어 체력 저장
                    // 큐가 비어있다면 플레이어 턴으로 전환
                    isDF = false; // 방어 상태 해제
                    GameManager.Instance.currentState = DungeonState.PlayerTrun;
                    Print("\ninfo : 몬스터의 공격이 끝났습니다");
                    // 입력 버퍼 비우기
                    while (Console.KeyAvailable) Console.ReadKey(true);

                    Thread.Sleep(300);

                    // 또 한 번 비워주기 (남아있을 수도 있으니까)
                    while (Console.KeyAvailable) Console.ReadKey(true);
                }
            }
        }

        void EnemyAttack(int index)
        {
            if (currentMonsters[index].Stat.CurrentHp==0)
                return; // 몬스터가 죽어있다면 공격하지 않음

            if (myPlayer.Stat.IsDead)
            {
                isDF= false; // 방어 상태 해제
                isWin = false; // 플레이어가 죽었을 때 패배 상태로 변경
                GameManager.Instance.currentState = DungeonState.EndBattle;
                Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey(true); // 유저가 키를 누를 때까지 대기
            }
            else
            {
                if (isDF)
                    myPlayer.Defend(currentMonsters[index]);

                else
                    currentMonsters[index].Attack(myPlayer);
                Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey(true); // 유저가 키를 누를 때까지 대기;
            }
        }

        // ==============[전투 결과상태]==============// 여기부터 구현하면댐

        void EndBattleRender()
        {
            if(isWin)
            {
                WinRender();
            }
            else
            {
                LoseRender();
            }
        }

        void WinRender()
        {
            Print("◎Battle!! - Result◎", ConsoleColor.DarkYellow);
            Print($"\nVictory!\n", ConsoleColor.Green);

            Print($"던전에서 몬스터 {currentMonsters.Count}마리를 잡았습니다.\n");
            Print($"Lv.{myPlayer.Stat.Level} | {myPlayer.Name}");
            Print($"HP.{dungeonHP} -> {myPlayer.Stat.CurrentHp}\n");

            Print(0, "다음", ConsoleColor.DarkCyan);

            Print("\n원하시는 행동을 입력해주세요");
            Console.Write(">>");
        }

        void LoseRender()
        {
            Print("◎Battle!! - Result◎", ConsoleColor.DarkYellow);
            Print($"\nYou Lose!\n", ConsoleColor.Green);

            Print($"Lv.{myPlayer.Stat.Level} | {myPlayer.Name}\n");
            Print($"HP.{dungeonHP} -> {myPlayer.Stat.CurrentHp}\n");
            
            Print(0, "다음", ConsoleColor.DarkCyan);
            Print("\n원하시는 행동을 입력해주세요");
            Console.Write(">>");
        }
        void EndTrunMove(int index)
        {
            if (index < 0 || index > currentMonsters.Count)
            {
                Print("\ninfo : 잘못 입력 하셨습니다.");
                // 입력 버퍼 비우기
                while (Console.KeyAvailable) Console.ReadKey(true);

                Thread.Sleep(250);

                // 또 한 번 비워주기 (남아있을 수도 있으니까)
                while (Console.KeyAvailable) Console.ReadKey(true); return;
            }
            if (index == 0)
            {
                if (isWin)
                {
                    GameManager.Instance.currentState = DungeonState.Adventure;
                    currentMonsters.Clear(); // 몬스터 목록 초기화
                    dungeonHP = 0;
                    Print("\ninfo : 전투를 종료합니다.");
                    // 입력 버퍼 비우기
                    while (Console.KeyAvailable) Console.ReadKey(true);

                    Thread.Sleep(200);

                    // 또 한 번 비워주기 (남아있을 수도 있으니까)
                    while (Console.KeyAvailable) Console.ReadKey(true);
                }

                else
                {
                    int loseHP = (int)(myPlayer.Stat.MaxHp * 0.3f); // 플레이어가 죽었을 때 체력 감소
                    dungeonHP = 0;
                    Print("\ninfo : 쓰러진 당신은 던전마법으로 마을로 돌아갑니다.");
                    Print($"패배 패널티로 체력이 {loseHP}/{myPlayer.Stat.MaxHp}이 됩니다.");
                    GameManager.Instance.SwitchScene(GameState.TownScene); // 마을로 돌아가기
                    walkCount = 0;// 이동 횟수 초기화
                                  // 입력 버퍼 비우기
                    while (Console.KeyAvailable) Console.ReadKey(true);

                    Thread.Sleep(1500);

                    // 또 한 번 비워주기 (남아있을 수도 있으니까)
                    while (Console.KeyAvailable) Console.ReadKey(true); return;
                }
            }
        }
    }
}