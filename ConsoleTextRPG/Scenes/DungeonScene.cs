using ConsoleTextRPG.Data;
using ConsoleTextRPG.Managers;
using ConsoleTextRPG.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
        Queue<Monster> monsterQueue = new Queue<Monster>(); // 몬스터 공격 순서를 저장하는 큐
        List<Monster> currentMonsters = new List<Monster>();
        private int selectedSkillId = 0;

        public override void RenderMenu()
        {
            switch (GameManager.Instance.currentState)
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
                case DungeonState.PlayerTurn: // 오타 수정
                    PlayerTurnRender();
                    break;
                case DungeonState.PlayerAttack:
                    PlayerAttackRender();
                    break;
                case DungeonState.PlayerSkill:
                    PlayerSkillRender();
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
                    DungeonMove(index);
                    break;
                case DungeonState.PlayerTurn: // 오타 수정
                    PlayerTurnMove(index);
                    break;
                case DungeonState.PlayerAttack:
                    PlayerAttackMove(index);
                    break;
                case DungeonState.PlayerSkill:
                    PlayerSkillMove(index);
                    break;
                case DungeonState.EnemyTurn:
                    EnemyAttackMove(index);
                    break;
                case DungeonState.EndBattle:
                    EndTurnMove(index); // 오타 수정
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
                int num = (walkCount * 100) / dungeonClearCount;
                Print(1, $"계속하기 | 진행도({num}%)", ConsoleColor.DarkCyan);
                Print(2, "새로하기", ConsoleColor.DarkCyan);
            }
            else
            {
                Print(1, "새로하기", ConsoleColor.DarkCyan);
            }
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
                        GameManager.Instance.SwitchScene(GameState.TownScene);
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
                        walkCount = 0;
                        isAtive = false;
                        GameManager.Instance.currentLevel = DungeonLevel.Normal;
                        GameManager.Instance.currentState = DungeonState.SelectLevel;
                        Thread.Sleep(100);
                        break;
                    case 0:
                        Info("마을로 돌아갑니다");
                        GameManager.Instance.SwitchScene(GameState.TownScene);
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
                    GameManager.Instance.SwitchScene(GameState.TownScene);
                    Thread.Sleep(100);
                    break;
                default:
                    Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                    Thread.Sleep(200);
                    break;
            }
        }

        // ============================[던전상태]============================
        void DungeonRender()
        {
            deadCount = 0;
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
                    GameManager.Instance.SwitchScene(GameState.TownScene);
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
                walkCount++;
                if (new Random().NextDouble() < monsValue)
                {
                    GameManager.Instance.currentState = DungeonState.PlayerTurn; // 오타 수정
                    SwapnMonster();
                }
            }
            else
            {
                Console.WriteLine("\ninfo : 던전을 클리어했습니다.");
                Console.WriteLine("\ninfo : 마을로 돌아갑니다");
                GameManager.Instance.SwitchScene(GameState.TownScene);
                walkCount = 0;
                Thread.Sleep(1000);
                return;
            }
        }

        void SwapnMonster()
        {
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
                    min = 2;
                    max = 5;
                    break;
            }
            currentMonsters.Clear();
            var rnd = new Random();
            var types = GameManager.Instance.monsType.Keys.ToList();
            int MonsterCount = new Random().Next(min, max);

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
            dungeonHP = myPlayer.Stat.CurrentHp;
            Print("◎Battle!!◎", ConsoleColor.DarkYellow);
            Print($"\n몬스터가 {currentMonsters.Count}마리가 나타났습니다!\n");
            Print("\n============[몬스터]============");
            foreach (var monster in currentMonsters)
            {
                if (monster.Stat.IsDead)
                    monster.PrintMonster(ConsoleColor.DarkGray);
                else
                    monster.PrintMonster();
            }

            Print("===========[선택지]===========");
            Print(1, "기본 공격", ConsoleColor.DarkCyan);
            Print(2, "스킬 사용", ConsoleColor.DarkCyan);
            Print(3, "방어", ConsoleColor.DarkCyan);
            Print(4, "도망", ConsoleColor.DarkCyan);

            Print("\n원하시는 행동을 입력해주세요");
            Console.Write(">>");
        }

        void PlayerTurnMove(int index)
        {
            switch (index)
            {
                case 1:
                    GameManager.Instance.currentState = DungeonState.PlayerAttack;
                    selectedSkillId = 0;
                    break;
                case 2:
                    GameManager.Instance.currentState = DungeonState.PlayerSkill;
                    break;
                case 3:
                    Info("방어합니다");
                    PlayerDefend();
                    break;
                case 4:
                    PlayerRun();
                    break;
                default:
                    Print("\ninfo : 잘못 입력 하셨습니다.");
                    while (Console.KeyAvailable) Console.ReadKey(true);
                    Thread.Sleep(300);
                    while (Console.KeyAvailable) Console.ReadKey(true);
                    break;
            }
            // 플레이어 턴 종료 시 쿨타임 감소
            myPlayer.ReduceSkillCooldowns();
            if (GameManager.Instance.currentState != DungeonState.PlayerSkill &&
                GameManager.Instance.currentState != DungeonState.PlayerAttack)
            {
                GameManager.Instance.currentState = DungeonState.EnemyTurn;
            }
        }

        void PlayerDefend()
        {
            isDF = true;
            GameManager.Instance.currentState = DungeonState.EnemyTurn;
            MonstersDeadCheck();
        }

        void PlayerRun()
        {
            if (currentMonsters.Count > 1)
            {
                if (new Random().NextDouble() < 0.1f)
                {
                    if (new Random().NextDouble() < 0.3f)
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
            while (Console.KeyAvailable) Console.ReadKey(true);
            Thread.Sleep(500);
            while (Console.KeyAvailable) Console.ReadKey(true);
        }

        void LoseGold()
        {
            Info("도망치다가 소지금을 흘렸습니다");
            myPlayer.Gold -= (int)(myPlayer.Gold * 0.05f);
        }

        // ============================[플레이어 공격 상태]============================
        void PlayerSkillRender()
        {
            Print("==◎ Battle!! ◎==", ConsoleColor.DarkYellow);
            Print($"\n사용할 스킬을 선택해주세요.\n");
            Print("\n============[스킬]============");
            for (int i = 0; i < myPlayer.Skills.Count; i++)
            {
                var skill = myPlayer.Skills[i];
                if (skill.CurrentCooldown > 0)
                {
                    Print(i + 1, $"{skill.Name} (데미지: {skill.Damage}, 쿨타임: {skill.CurrentCooldown}턴)", ConsoleColor.DarkGray);
                }
                else
                {
                    Print(i + 1, $"{skill.Name} (데미지: {skill.Damage})", ConsoleColor.DarkCyan);
                }
            }
            Print(0, "취소", ConsoleColor.DarkCyan);
            Print("\n원하시는 스킬의 번호를 입력해주세요");
            Console.Write(">>");
        }

        void PlayerSkillMove(int index)
        {
            Console.WriteLine($"PlayerSkillMove 호출: 입력된 index = {index}, 스킬 수 = {myPlayer.Skills.Count}");
            if (index < 0 || index > myPlayer.Skills.Count)
            {
                Print("\ninfo : 잘못 입력 하셨습니다.");
                while (Console.KeyAvailable) Console.ReadKey(true);
                Thread.Sleep(100);
                while (Console.KeyAvailable) Console.ReadKey(true);
                GameManager.Instance.currentState = DungeonState.PlayerTurn;
                return;
            }

            if (index == 0)
            {
                Print("스킬 사용을 취소합니다.");
                while (Console.KeyAvailable) Console.ReadKey(true);
                Thread.Sleep(100);
                while (Console.KeyAvailable) Console.ReadKey(true);
                GameManager.Instance.currentState = DungeonState.PlayerTurn;
                return;
            }

            var selectedSkill = myPlayer.Skills[index - 1];
            if (selectedSkill.CurrentCooldown > 0)
            {
                Print($"\ninfo : {selectedSkill.Name}은(는) 쿨타임 중입니다! (남은 턴: {selectedSkill.CurrentCooldown})");
                while (Console.KeyAvailable) Console.ReadKey(true);
                Thread.Sleep(300);
                while (Console.KeyAvailable) Console.ReadKey(true);
                GameManager.Instance.currentState = DungeonState.PlayerTurn;
                return;
            }

            selectedSkillId = selectedSkill.Id;
            Console.WriteLine($"선택된 스킬 ID: {selectedSkillId}");
            GameManager.Instance.currentState = DungeonState.PlayerAttack;
        }

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
            if (index < 0 || index > currentMonsters.Count)
            {
                Print("\ninfo : 잘못 입력 하셨습니다.");
                while (Console.KeyAvailable) Console.ReadKey(true);
                Thread.Sleep(300);
                while (Console.KeyAvailable) Console.ReadKey(true);
                return;
            }

            if (index == 0)
            {
                GameManager.Instance.currentState = DungeonState.PlayerTurn;
                Info("공격을 취소합니다.");
                while (Console.KeyAvailable) Console.ReadKey(true);
                Thread.Sleep(300);
                while (Console.KeyAvailable) Console.ReadKey(true);
                return;
            }

            if (currentMonsters[index - 1].Stat.CurrentHp <= 0)
            {
                Print("\ninfo : 이미 죽은 몬스터입니다.");
                while (Console.KeyAvailable) Console.ReadKey(true);
                Thread.Sleep(300);
                while (Console.KeyAvailable) Console.ReadKey(true);
                return;
            }

            if (selectedSkillId != 0)
            {
                myPlayer.UseSkill(selectedSkillId, currentMonsters[index - 1]);
                selectedSkillId = 0;
            }
            else
            {
                bool wasAlive = !currentMonsters[index - 1].Stat.IsDead;
                myPlayer.Attack(currentMonsters[index - 1]);
                if (wasAlive && currentMonsters[index - 1].Stat.IsDead)
                {
                    QuestManager.Instance.OnMonsterKilled(currentMonsters[index - 1].Name);
                }
            }

            MonstersDeadCheck();
        }

        void PlayerAttack(int index)
        {
            bool wasAlive = !currentMonsters[index].Stat.IsDead;
            myPlayer.Attack(currentMonsters[index]);
            bool isDeadNow = currentMonsters[index].Stat.IsDead;
            if (wasAlive && isDeadNow)
            {
                QuestManager.Instance.OnMonsterKilled(currentMonsters[index].Name);
            }
            MonstersDeadCheck();
        }

        void MonstersDeadCheck()
        {
            deadCount = 0;
            monsterQueue.Clear();
            foreach (var monster in currentMonsters)
            {
                if (!monster.Stat.IsDead)
                {
                    monsterQueue.Enqueue(monster);
                }
            }

            foreach (var monster in currentMonsters)
            {
                if (monster.Stat.IsDead)
                    deadCount++;

                if (deadCount == currentMonsters.Count)
                {
                    isWin = true;
                    GameManager.Instance.currentState = DungeonState.EndBattle;
                    Info("모든 몬스터를 처치했습니다.");
                }
                else
                {
                    GameManager.Instance.currentState = DungeonState.EnemyTurn;
                }
            }
            Console.WriteLine("계속하려면 아무 키나 누르세요...");
            Console.ReadKey(true);
        }

        // ============================[몬스터턴상태]============================
        void EnemyTurnRender()
        {
            Print("◎Battle!!◎", ConsoleColor.DarkYellow);
            Print($"\n몬스터의 공격이 시작됩니다!\n");
            Print("\n============[몬스터]============");
            foreach (var monster in currentMonsters)
            {
                if (monster.Stat.IsDead) // 오타 수정: isDead -> Stat.IsDead
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
                while (Console.KeyAvailable) Console.ReadKey(true);
                Thread.Sleep(200);
                while (Console.KeyAvailable) Console.ReadKey(true);
                return;
            }

            if (index == 0)
            {
                if (monsterQueue.Count > 0)
                {
                    var nextMonster = monsterQueue.Dequeue();
                    int idx = currentMonsters.IndexOf(nextMonster);
                    EnemyAttack(idx);
                }
                else
                {
                    dungeonHP = myPlayer.Stat.CurrentHp;
                    isDF = false;
                    GameManager.Instance.currentState = DungeonState.PlayerTurn;
                    Print("\ninfo : 몬스터의 공격이 끝났습니다");
                    while (Console.KeyAvailable) Console.ReadKey(true);
                    Thread.Sleep(300);
                    while (Console.KeyAvailable) Console.ReadKey(true);
                }
            }
        }

        void EnemyAttack(int index)
        {
            if (currentMonsters[index].Stat.CurrentHp == 0)
                return;

            if (myPlayer.Stat.IsDead)
            {
                isDF = false;
                isWin = false;
                GameManager.Instance.currentState = DungeonState.EndBattle;
                Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey(true);
            }
            else
            {
                if (isDF)
                    myPlayer.Defend(currentMonsters[index]);
                else
                    currentMonsters[index].Attack(myPlayer);
                Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey(true);
            }
        }

        // ============================[전투 결과상태]============================
        void EndBattleRender()
        {
            if (isWin)
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

        void EndTurnMove(int index)
        {
            if (index < 0 || index > currentMonsters.Count)
            {
                Print("\ninfo : 잘못 입력 하셨습니다.");
                while (Console.KeyAvailable) Console.ReadKey(true);
                Thread.Sleep(250);
                while (Console.KeyAvailable) Console.ReadKey(true);
                return;
            }
            if (index == 0)
            {
                if (isWin)
                {
                    GameManager.Instance.currentState = DungeonState.Adventure;
                    currentMonsters.Clear();
                    dungeonHP = 0;
                    Print("\ninfo : 전투를 종료합니다.");
                    while (Console.KeyAvailable) Console.ReadKey(true);
                    Thread.Sleep(200);
                    while (Console.KeyAvailable) Console.ReadKey(true);
                }
                else
                {
                    int loseHP = (int)(myPlayer.Stat.MaxHp * 0.3f);
                    dungeonHP = 0;
                    Print("\ninfo : 쓰러진 당신은 던전마법으로 마을로 돌아갑니다.");
                    Print($"패배 패널티로 체력이 {loseHP}/{myPlayer.Stat.MaxHp}이 됩니다.");
                    myPlayer.Stat.CurrentHp = loseHP; // 패배 시 체력 설정
                    GameManager.Instance.SwitchScene(GameState.TownScene);
                    walkCount = 0;
                    while (Console.KeyAvailable) Console.ReadKey(true);
                    Thread.Sleep(1500);
                    while (Console.KeyAvailable) Console.ReadKey(true);
                }
            }
        }
    }
}