using ConsoleTextRPG.Data;
using ConsoleTextRPG.Managers;
using ConsoleTextRPG.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
                case DungeonState.Select:
                    SelectRender(); 
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

                case DungeonState.Select:
                    SelectMove(index);
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
                case DungeonState.PlayerSkill: // 플레이어 스킬 행동 선택
                    PlayerSkillMove(index);
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
        void SelectRender()
        {
            dungeonHP = myPlayer.Stat.CurrentHp; // 현재 플레이어 체력 저장
            deadCount = 0; // 죽은 몬스터 수 초기화
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
        void SelectMove(int index)
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
            dungeonHP = myPlayer.Stat.CurrentHp; // 현재 플레이어 체력 저장
            deadCount = 0; // 죽은 몬스터 수 초기화
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

        // ============================[플레이어턴상태]============================
        void PlayerTurnRender()
        {
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
            Print(1, "기본 공격", ConsoleColor.DarkCyan);
            Print(2, "스킬 사용", ConsoleColor.DarkCyan);
            Print(3, "방어", ConsoleColor.DarkCyan);
            Print(4, "도망", ConsoleColor.DarkCyan);

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
                    Console.WriteLine("\ninfo : 잘못 입력 하셨습니다.");
                    Thread.Sleep(200);
                    break;
            }
        }

        void ShowSkillList()
        {
            Console.Clear();
            Console.WriteLine("==Skill 사용==");
            Console.WriteLine("사용할 스킬을 선택해주세요\n");
            Console.WriteLine("0. 취소");

            for (int i = 0; i < GameManager.Instance.Player.Skills.Count; i++)
            {
                SkillManager skill = GameManager.Instance.Player.Skills[i];
                Console.WriteLine($"{i + 1}. {skill.Name} (MP {skill.ManaCost})");
            }

            Console.Write(">> ");
        }

        void PlayerSkillRender()
        {
            Print("==Skill 사용==", ConsoleColor.Cyan);
            Print("사용할 스킬을 선택해주세요\n");

            for (int i = 0; i < GameManager.Instance.Player.Skills.Count; i++)
            {
                var skill = GameManager.Instance.Player.Skills[i];
                Print(i + 1, $"{skill.Name} (MP {skill.ManaCost}) - {skill.Description}");
            }

            Print(0, "취소", ConsoleColor.DarkGray);
            Console.Write("\n>> ");
        }

        void PlayerSkillMove(int index)
        {
            if (index == 0)
            {
                Info("스킬 선택을 취소합니다.");
                GameManager.Instance.currentState = DungeonState.PlayerTrun;
                return;
            }


            // player 객체와 몬스터 리스트를 사용
            SkillManager.UseSkill(myPlayer, index - 1, currentMonsters[0]);

            MonstersDeadCheck();
            GameManager.Instance.currentState = DungeonState.EnemyTurn;


            void PlayerDefend()
        {
            isDF= true; // 방어 상태로 변경
            GameManager.Instance.currentState = DungeonState.EnemyTurn;
            MonstersDeadCheck();
        }

        void PlayerRun()
        {
            if(currentMonsters.Count > 1)
            {
                if (new Random().NextDouble() < 0.3f) // 30% 확률로 도망 성공
                {
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
                GameManager.Instance.currentState = DungeonState.Adventure;
                Info("도망쳤습니다.");
                GameManager.Instance.currentState = DungeonState.Adventure;
                Thread.Sleep(200);
            }
        

        // ============================[플레이어 공격 상태]============================
<<<<<<< Updated upstream
=======
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

>>>>>>> Stashed changes
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
                Thread.Sleep(300);
                return;
            }

            if( index == 0)
            {
                GameManager.Instance.currentState = DungeonState.PlayerTrun; // 공격 취소시 플레이어 턴으로 돌아감
                Info("공격을 취소합니다.");
                Thread.Sleep(300);
                return;
            }
<<<<<<< Updated upstream
            else
            {
                if (currentMonsters[index-1].Stat.CurrentHp <= 0)
                {
                    Print("\ninfo : 이미 죽은 몬스터입니다.");
                    Thread.Sleep(300);
                    return;
=======

            // 스킬이 선택된 경우
            if (selectedSkillId != 0)
            {
                var skill = myPlayer.Skills.Find(s => s.Id == selectedSkillId);
                if (skill != null)
                {
                    // 힐 스킬이면 대상 선택 없이 자기 자신에게 적용
                    if (skill.Effect == "Heal")
                    {
                        bool used = skill.Use(myPlayer, myPlayer); // 대상은 자기 자신
                        if (!used)
                        {
                            GameManager.Instance.currentState = DungeonState.PlayerSkill;
                            return;
                        }
                        selectedSkillId = 0;
                    }
                    else
                    {
                        // 힐이 아니면 기존대로 몬스터 대상으로 사용
                        if (index == 0)
                        {
                            Print("\ninfo : 공격 취소됨");
                            GameManager.Instance.currentState = DungeonState.PlayerTurn;
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

                        bool used = skill.Use(myPlayer, currentMonsters[index - 1]);
                        if (!used)
                        {
                            GameManager.Instance.currentState = DungeonState.PlayerSkill;
                            return;
                        }
                        selectedSkillId = 0;
                    }
>>>>>>> Stashed changes
                }
                else
                {
                    PlayerAttack(index - 1);
                }
            }
<<<<<<< Updated upstream
        }

        void PlayerAttack(int index)
        {
            // 중독상태 같은 상태이상 공격이 있을 경우는 플레이어 공격전에 DeadCheck를 먼저 실행해야함
            myPlayer.Attack(currentMonsters[index]);
            MonstersDeadCheck();
        
        
            Monster target = currentMonsters[index];
            int baseDamage = Character.CharacterStat.Stat.atk;

            Random rand = new Random();
            bool isCritical = rand.Next(0, 7) == 0; // 1/7 확률

            int finalDamage = baseDamage;

            if (isCritical)
            {
                finalDamage *= 3;
                Print("\n[치명타 발생!] 데미지가 3배 증가합니다!", ConsoleColor.Red);
            }

            target.Stat.CurrentHp -= finalDamage;

            Print($"{target.Name}에게 {finalDamage}의 피해를 입혔습니다.", ConsoleColor.Yellow);

            if (target.Stat.CurrentHp <= 0)
            {
                target.Stat.CurrentHp = 0;
                Print($"{target.Name}을(를) 처치했습니다!", ConsoleColor.Green);
            }

            Thread.Sleep(500);
            GameManager.Instance.currentState = DungeonState.EnemyTurn;
        }


=======
            else
            {
                if (index == 0)
                {
                    GameManager.Instance.currentState = DungeonState.PlayerTurn;
                    Info("공격을 취소합니다.");
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

                bool wasAlive = !currentMonsters[index - 1].Stat.IsDead;
                myPlayer.Attack(currentMonsters[index - 1]);
                if (wasAlive && currentMonsters[index - 1].Stat.IsDead)
                {
                    QuestManager.Instance.OnMonsterKilled(currentMonsters[index - 1].Name);
                }
            }

            MonstersDeadCheck();
        }

>>>>>>> Stashed changes
        void MonstersDeadCheck()
        {
            // 다음 몬스터턴 행동에 사용될 살아있는 몬스터들 큐에 추가
            foreach (var monster in currentMonsters)
            {
                if (!monster.isDead)               //  살아있는 몬스터 필터링
                {
                    monsterQueue.Enqueue(monster);
                }
            }

            // 몬스터들 전부 죽었는지  확인
            foreach (var monster in currentMonsters)
            {
                if (monster.Stat.CurrentHp == 0)
                    deadCount++;

                if (deadCount == currentMonsters.Count)
                {
                    isWin = true; // 모든 몬스터를 처치했을 때 승리 상태로 변경
                    GameManager.Instance.currentState = DungeonState.EndBattle;
                    Info("모든 몬스터를 처치했습니다.");
                    Thread.Sleep(800);
                }
                else
                {
                    GameManager.Instance.currentState = DungeonState.EnemyTurn;
                    Thread.Sleep(800);
                }
            }
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
                Thread.Sleep(300);
                return;
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
                    // 큐가 비어있다면 플레이어 턴으로 전환
                    isDF = false; // 방어 상태 해제
                    GameManager.Instance.currentState = DungeonState.PlayerTrun;
                    Print("\ninfo : 몬스터의 공격이 끝났습니다");
                    Thread.Sleep(800);
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
                Thread.Sleep(800);
            }
            else
            {
                if (isDF)
                    myPlayer.Defend(currentMonsters[index]);

                else
                    currentMonsters[index].Attack(myPlayer);
                Thread.Sleep(1000);
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
                Thread.Sleep(300);
                return;
            }
            if (index == 0)
            {
                if (isWin)
                {
                    GameManager.Instance.currentState = DungeonState.Adventure;
                    currentMonsters.Clear(); // 몬스터 목록 초기화
                    dungeonHP = 0;
                    Print("\ninfo : 전투를 종료합니다.");
                    Thread.Sleep(300);
                }

                else
                {
                    int loseHP = (int)(dungeonHP * 0.5f); // 플레이어가 죽었을 때 체력 감소
                    myPlayer.Stat.CurrentHp += loseHP;
                    dungeonHP = 0;
                    Print("\ninfo : 쓰러진 당신은 던전마법으로 마을로 돌아갑니다.");
                    Print($"패배 패널티로 체력이 {myPlayer.Stat.CurrentHp}/{myPlayer.Stat.MaxHp}이 됩니다.");
                    GameManager.Instance.SwitchScene(GameState.TownScene); // 마을로 돌아가기
                    walkCount = 0;// 이동 횟수 초기화
                    Thread.Sleep(1000);
                    return;
                }
            }
        }
    }
}