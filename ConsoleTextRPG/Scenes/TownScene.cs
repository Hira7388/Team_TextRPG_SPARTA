using System;

namespace ConsoleTextRPG.Scenes
{
    internal class TownScene
    {
        // 싱글톤 인스턴스
        private static TownScene _instance;
        public static TownScene Instance => _instance ??= new TownScene();

        // 캐릭터 클래스
        public class Player
        {
            public string Name { get; set; } = string.Empty;
            public int Level { get; set; } = 1;
            public string Job { get; set; }  // Class -> Job
            public int Gold
            {
                get => gold;
                set => gold = value >= 0 ? value : 0;
            }
            public int Attack { get; set; }
            public int Defense { get; set; }
            public int Health { get; set; }

            // 아이템 효과
            public int ItemAttack { get; set; }
            public int ItemDefense { get; set; }
            public int ItemHealth { get; set; }

            private int gold;
        }

        // 플레이어 인스턴스
        public Player player = new Player();

        // 생성자
        private TownScene() { }

        // 마을 진입
        public void Enter()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 것을 환영합니다!");

<<<<<<< Updated upstream
            if (string.IsNullOrWhiteSpace(intinput))
=======
            if (string.IsNullOrWhiteSpace(input))
>>>>>>> Stashed changes
            {
                AskPlayerName();  // 이름 입력
                ChooseJob();      // 직업 선택
            }

            ShowMenu();           // 마을 메뉴
        }

        // 이름 입력
        private void AskPlayerName()
        {
            while (true)
            {
                Console.Write("\n당신의 이름은 기억이 나십니까? ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("⚠️  유효하지 않은 이름입니다. 다시 입력해주세요.");
                    Console.ReadKey();
                }
                else
                {
                    player.Name = input;
                    Console.WriteLine($"나의 이름은, {player.Name}...\n");
                    break;
                }
            }
        }

        // 직업 선택
        private void ChooseJob()
        {
            while (true)
            {
                Console.WriteLine("직업을 선택하세요:");
                Console.WriteLine("1. 전사");
                Console.WriteLine("2. 마법사");
                Console.Write("선택: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    player.Job = "전사";
                    player.Level = 1;
                    player.Gold = 100;
                    player.Attack = 15;
                    player.Defense = 10;
                    player.Health = 120;
                    Console.WriteLine($"{player.Name}님은 용맹한 전사가 되었습니다!\n");
                    break;
                }
                else if (choice == "2")
                {
                    player.Job = "마법사";
                    player.Level = 1;
                    player.Gold = 100;
                    player.Attack = 20;
                    player.Defense = 5;
                    player.Health = 80;
                    Console.WriteLine($"{player.Name}님은 지혜로운 마법사가 되었습니다!\n");
                    break;
                }
                else
                {
                    Console.WriteLine("⚠️  올바른 번호를 선택해주세요.");
                    Console.ReadKey();
                }
            }
        }

        // 마을 메뉴
        private void ShowMenu()
        {
            bool stayInTown = true;

            while (stayInTown)
            {
                Console.Clear();
                Console.WriteLine("어디로 가시겠습니까?");
                Console.WriteLine("1. 상점");
                Console.WriteLine("2. 던전");
                Console.WriteLine("3. 내 정보 보기");
                Console.WriteLine("4. 게임 종료");

                Console.Write("선택: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("\n[상점에 입장했습니다. (추후 구현)]");
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.WriteLine("\n[던전으로 향합니다... (추후 구현)]");
                        Console.ReadKey();
                        break;
                    case "3":
                        ShowStatus();
                        break;
                    case "4":
                        Console.WriteLine("\n게임을 종료합니다. 안녕히 가세요!");
                        stayInTown = false;
                        break;
                    default:
                        Console.WriteLine("⚠️  올바른 번호를 선택해주세요.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // 내 정보 보기
        private void ShowStatus()
        {
            Console.Clear();
            Console.WriteLine("📜 [내 정보]");
            Console.WriteLine($"이름   : {player.Name}");
            Console.WriteLine($"레벨   : {player.Level}");
            Console.WriteLine($"직업   : {player.Job}");
            Console.WriteLine($"소지금 : {player.Gold} G");
            Console.WriteLine($"공격력 : {player.Attack}{(player.ItemAttack > 0 ? $"(+{player.ItemAttack})" : "")}");
            Console.WriteLine($"방어력 : {player.Defense}{(player.ItemDefense > 0 ? $"(+{player.ItemDefense})" : "")}");
            Console.WriteLine($"체력   : {player.Health}{(player.ItemHealth > 0 ? $"(+{player.ItemHealth})" : "")}");
            Console.WriteLine("\n메뉴로 돌아가려면 아무 키나 누르세요...");
            Console.ReadKey();
        }
    }
}
