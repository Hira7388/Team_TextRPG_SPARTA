using System;

namespace ConsoleTextRPG.Scenes
{
    internal class TownScene
    {
        // 싱글톤
        private static TownScene _instance;
        public static TownScene Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TownScene();
                return _instance;
            }
        }

        // 생성자
        private TownScene() { }

        // 🧍‍♂️ 플레이어 정보 (7개)
        private string playerName;
        private int playerLevel;
        private string playerJob;
        private int playerGold;
        private int playerAtk;
        private int playerDef;
        private int playerHp;

        // 타운 진입
        public void Enter()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 것을 환영합니다!");

            AskPlayerName();     // 이름
            ChooseJob();         // 직업 (능력치 설정)
            ShowMenu();          // 메뉴
        }

        // 이름 입력
        private void AskPlayerName()
        {
            while (true)
            {
                Console.Write("당신의 이름을 입력하세요: ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("⚠️  유효하지 않은 이름입니다. 다시 입력해주세요.");
                }
                else
                {
                    playerName = input;
                    Console.WriteLine($"환영합니다, {playerName}님!\n");
                    break;
                }
            }
        }

        // 직업 선택 및 능력치 설정
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
                    playerJob = "전사";
                    playerLevel = 1;
                    playerGold = 100;
                    playerAtk = 15;
                    playerDef = 10;
                    playerHp = 120;
                    Console.WriteLine($"{playerName}님은 용맹한 전사가 되었습니다!\n");
                    break;
                }
                else if (choice == "2")
                {
                    playerJob = "마법사";
                    playerLevel = 1;
                    playerGold = 100;
                    playerAtk = 20;
                    playerDef = 5;
                    playerHp = 80;
                    Console.WriteLine($"{playerName}님은 지혜로운 마법사가 되었습니다!\n");
                    break;
                }
                else
                {
                    Console.WriteLine("⚠️  올바른 번호를 선택해주세요.\n");
                }
            }
        }

        // 마을 메뉴
        private void ShowMenu()
        {
            bool stayInTown = true;

            while (stayInTown)
            {
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
                        Console.WriteLine("\n[상점에 입장했습니다.]\n");
                        break;
                    case "2":
                        Console.WriteLine("\n[던전으로 향합니다...]\n");
                        break;
                    case "3":
                        ShowStatus();
                        break;
                    case "4":
                        Console.WriteLine("\n게임을 종료합니다. 안녕히 가세요!");
                        stayInTown = false;
                        break;
                    default:
                        Console.WriteLine("⚠️  올바른 번호를 선택해주세요.\n");
                        break;
                }
            }
        }

        // 플레이어 정보 출력
        private void ShowStatus()
        {
            Console.WriteLine("\n📜 [내 정보]");
            Console.WriteLine($"이름   : {playerName}");
            Console.WriteLine($"레벨   : {playerLevel}");
            Console.WriteLine($"직업   : {playerJob}");
            Console.WriteLine($"소지금 : {playerGold} G");
            Console.WriteLine($"공격력 : {playerAtk}"); 
            Console.WriteLine($"방어력 : {playerDef}");
            Console.WriteLine($"체력   : {playerHp}\n");
        }
    }
}