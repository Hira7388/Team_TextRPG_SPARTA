using ConsoleTextRPG.Managers;
using ConsoleTextRPG.Data;
using System;
using System.Numerics;
using ConsoleTextRPG.Scenes;

namespace ConsoleTextRPG.Scenes
{
    public class TownScene : BaseScene
    {
        // 싱글톤 인스턴스
        //private static TownScene _instance;
        //public static TownScene Instance => _instance ??= new TownScene();

        // 플레이어 정보 클래스
        // 정진규 - Town씬에서 Player 객체를 만들 필요 없습니다. GameManager에서 인스턴스로 생성되기 때문에 불러오면 됩니다.
        Player myPlayer = GameManager.Instance.Player; // 게임매니저에서 생성된 Player 객체 불러오기

        //private class Player
        //{
        //    public string Name { get; set; } = string.Empty;
        //    public int Level { get; set; } = 1;
        //    public string Class { get; set; }
        //    public int Gold
        //    {
        //        get => gold;
        //        set => gold = value >= 0 ? value : 0;
        //    }
        //    public int Attack { get; set; }
        //    public int Defense { get; set; }
        //    public int Health { get; set; }

        //    // 아이템 효과
        //    public int ItemAttack { get; set; }
        //    public int ItemDefense { get; set; }
        //    public int ItemHealth { get; set; }

        //    private int gold;
        //}

        //// 플레이어 인스턴스
        //private Player player = new Player();

        // 생성자
        //private TownScene() { }

        // 마을 진입

        // Render()에 화면 띄우기, Update()에 입력 받기
        public override void RenderMenu()
        {
            Enter();
        }
        public override void UpdateInput()
        {
            EnterInput();
        }


        private void Enter()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 것을 환영합니다!");

            if (!string.IsNullOrWhiteSpace(myPlayer.Name)) // 이름이 설정된 경우에만 마을 메뉴를 출력하도록 변경
            {
                ShowMenuOptions();  // 마을 메뉴
            }
        }
        private void EnterInput()
        {
            if (string.IsNullOrWhiteSpace(myPlayer.Name))
            {
                AskPlayerName();
                return; // 이름이 없으면 다음으로 안 넘어감
            }

            if (string.IsNullOrWhiteSpace(myPlayer.Job))
            {
                ChooseJob();
                return; // 직업이 없으면 메뉴 입력 안 받음
            }

            MenuInputActions(); // 이름과 직업이 모두 있으면 마을 메뉴로 이동

        }

        // 이름 입력
        private void AskPlayerName()
        {
            Console.Clear();
            Console.Write("\n당신의 이름은 기억이 나십니까? ");
            Console.Write("\n>> ");
            string input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("오류 : 유효하지 않은 이름입니다. 다시 입력해주세요.");
                // 입력 버퍼 비우기
                while (Console.KeyAvailable) Console.ReadKey(true);

                Thread.Sleep(300);

                // 또 한 번 비워주기 (남아있을 수도 있으니까)
                while (Console.KeyAvailable) Console.ReadKey(true);
            }
            else
            {
                myPlayer.SetName(input);

                Console.WriteLine($"\n나의 이름은, {myPlayer.Name}...\n");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }

        // 직업 선택
        private void ChooseJob()
        {
            Console.Clear();
            Console.WriteLine("직업을 선택하세요:");
            Console.WriteLine("1. 전사");
            Console.WriteLine("2. 마법사");
            Console.Write("선택: ");
            string choice = Console.ReadLine()?.Trim();



            // 직업 선택 성공 여부 체크용 플래그
            bool isValidChoice = false;


            if (choice == "1")
            {
                myPlayer.SetJob("전사");
                // 우선 Player.cs에서 기본 스텟을 제공(차후 직업별 스텟을 다르게 설정할 수 있음)
                //player.Level = 1;
                //player.Gold = 100;
                //player.Attack = 15;
                //player.Defense = 10;
                //player.Health = 120;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"{myPlayer.Name}님은 용맹한 전사가 되었습니다!\n");
                Console.ResetColor();

                isValidChoice = true;


            }
            else if (choice == "2")
            {
                myPlayer.SetJob("마법사");
                //player.Level = 1;
                //player.Gold = 100;
                //player.Attack = 20;
                //player.Defense = 5;
                //player.Health = 80;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"{myPlayer.Name}님은 지혜로운 마법사가 되었습니다!\n");
                Console.ResetColor();

                isValidChoice = true;


            }
            else
            {
                Console.WriteLine("오류 : 올바른 번호를 선택해주세요.");
                // 입력 버퍼 비우기
                while (Console.KeyAvailable) Console.ReadKey(true);

                Thread.Sleep(200);

                // 또 한 번 비워주기 (남아있을 수도 있으니까)
                while (Console.KeyAvailable) Console.ReadKey(true);

            }

            if (choice == "1")
            {
                myPlayer.SetJob("전사");
                Console.Clear();
                Console.WriteLine($"{myPlayer.Name}님은 용맹한 전사가 되었습니다!");
                Console.WriteLine("\n이 길이 나를 발할라로 인도하기를...");
                // 입력 버퍼 비우기
                while (Console.KeyAvailable) Console.ReadKey(true);

                Thread.Sleep(1000);

                // 또 한 번 비워주기 (남아있을 수도 있으니까)
                while (Console.KeyAvailable) Console.ReadKey(true); Console.Clear();
            }
            else if (choice == "2")
            {
                myPlayer.SetJob("마법사");
                Console.Clear();
                Console.WriteLine($"{myPlayer.Name}님은 지혜로운 마법사가 되었습니다!");
                Console.WriteLine("\n저 눈부신 빛이 저를 인도할 것입니다...");
                // 입력 버퍼 비우기
                while (Console.KeyAvailable) Console.ReadKey(true);

                Thread.Sleep(1000);

                // 또 한 번 비워주기 (남아있을 수도 있으니까)
                while (Console.KeyAvailable) Console.ReadKey(true); Console.Clear();
            }

            ShowMenuOptions();
            

        }
        

        // 마을 메뉴 옵션
        private void ShowMenuOptions()
        {
            Console.WriteLine("어디로 가시겠습니까?");
            Console.WriteLine("1. 상점");
            Console.WriteLine("2. 던전");
            Console.WriteLine("3. 내 정보 보기");
            Console.WriteLine("4. 인벤토리");
            Console.WriteLine("5. 저장하기");
            Console.WriteLine("6. 퀘스트 보러가기");
            Console.WriteLine("0. 게임 종료하기");

            Console.Write("선택: ");
        }
        private void MenuInputActions()
        {
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\n[상점에 입장했습니다.]");
                    //Console.ReadKey();
                    Thread.Sleep(500);
                    //Console.Clear();
                    GameManager.Instance.SwitchScene(GameState.StoreScene);
                    break;
                case "2":
                    Console.WriteLine("\n[던전으로 향합니다...]");
                    //Console.ReadKey();
                    Thread.Sleep(500);
                    //Console.Clear();
                    GameManager.Instance.SwitchScene(GameState.DungeonScene);
                    break;
                case "3":
                    ShowStatus();
                    break;
                case "4":
                    Console.WriteLine("\n[인벤토리로 향합니다.]");
                    //Console.ReadKey();
                    Thread.Sleep(500);
                    //Console.Clear();
                    GameManager.Instance.SwitchScene(GameState.InventoryScene);
                    break;
                case "5":
                    GameManager.Instance.SaveGame();
                    Info("게임이 저장되었습니다.");
                    Thread.Sleep(500); 
                    break;
                case "6":
                    Console.WriteLine("\n[퀘스트 공고로 향합니다.]");
                    Thread.Sleep(500);
                    GameManager.Instance.SwitchScene(GameState.QuestScene);
                    break;
                case "0":
                    GameManager.Instance.GameOver("\ninfo : 게임을 종료합니다. 안녕히 가세요!");
                    break;
                default:
                    Console.WriteLine("올바른 번호를 선택해주세요.");
                    Console.ReadKey();
                    break;
            }
        }

        // 내 정보 보기
        private void ShowStatus()
        {
            Console.Clear();
            Console.WriteLine("[내 정보]");
            Console.WriteLine($"이름   : {myPlayer.Name}");
            Console.WriteLine($"레벨   : {myPlayer.Stat.Level}");
            Console.WriteLine($"직업   : {myPlayer.Job}");
            Console.WriteLine($"소지금 : {myPlayer.Gold} G");
            Console.WriteLine($"공격력 : {myPlayer.Stat.BaseAttack}{(myPlayer.Stat.AdditionalAttack > 0 ? $"(+{myPlayer.Stat.AdditionalAttack})" : "")}");
            Console.WriteLine($"방어력 : {myPlayer.Stat.BaseDefense}{(myPlayer.Stat.AdditionalDefense > 0 ? $"(+{myPlayer.Stat.AdditionalDefense})" : "")}");
            Console.WriteLine($"체력   : {myPlayer.Stat.MaxHp}");
            // 아래는 아직 아이템으로 인한 추가 최대 체력이 없어서 잠시 비활성화 해두었습니다.
            //Console.WriteLine($"체력   : {myPlayer.Stat.MaxHp}{(myPlayer.Stat.AdditionalHp > 0 ? $"(+{myPlayer.ItemHealth})" : "")}"); 
            Console.WriteLine("\n메뉴로 돌아가려면 아무 키나 누르세요...");
            Console.ReadKey();
        }
    }
}
