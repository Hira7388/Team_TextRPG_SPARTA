using ConsoleTextRPG.Scenes;
using ConsoleTextRPG.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleTextRPG.Monsters;
using Newtonsoft.Json;

namespace ConsoleTextRPG.Managers
{
    internal class GameManager
    {
        // 현재 씬
        private GameState currentScene;

        private readonly Dictionary<GameState, BaseScene> scenes = new();

        // 플레이어 객체 생성
        public Player Player { get; private set; }

        public readonly Dictionary<MonsterType, Monster> monsType = new();

        // 모든 아이템 정보 불러오기
        public List<Item> AllItems { get; private set; }

        // 현재 던전 난이도
        private DungeonLevel _currentLevel;
        public DungeonLevel currentLevel { get; set; }

        // 현재 던전 상태
        private DungeonState _currentState;
        public DungeonState currentState { get; set; }

        // 싱글톤
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null) _instance = new GameManager();
                return _instance;
            }
        }

        // 생성자
        private GameManager()
        {

        }

        private bool running = true;

        public void GameRun()
        {
            Init();

            if (File.Exists(SaveManager.Instance.SaveFilePath))
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("저장된 게임이 있습니다. 불러오시겠습니까?");
                    Console.WriteLine("1. 불러오기");
                    Console.WriteLine("2. 새 게임 시작");
                    Console.Write(">> ");
                    string input = Console.ReadLine()?.Trim();

                    if (input == "1")
                    {
                        if (ConfirmWithNumbers("정말 저장된 게임을 불러오시겠습니까?", "불러오기"))
                        {
                            LoadGame();
                            break;
                        }
                    }
                    else if (input == "2")
                    {
                        if (ConfirmWithNumbers("정말 새 게임을 시작하시겠습니까?", "시작하기"))
                        {
                            Console.WriteLine("새 게임을 시작합니다.\n");
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다. 1 또는 2를 입력해주세요.");
                        Thread.Sleep(1000);
                    }
                }
            }

            while (running)
            {
                RenderMenu();
                UpdateInput();
            }
        }

        private bool ConfirmWithNumbers(string message, string confirmActionName)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"{message}");
                Console.WriteLine($"1. {confirmActionName}");
                Console.WriteLine("2. 취소");
                Console.Write(">> ");
                string input = Console.ReadLine()?.Trim();

                if (input == "1") return true;
                if (input == "2") return false;

                Console.WriteLine("잘못된 입력입니다. 1 또는 2를 입력해주세요.");
                Thread.Sleep(1000);
            }
        }


        private void Init()
        {
            this.Player = new Player("");
            Console.CursorVisible = false;
            LoadItemDatabase();
            if (SkillManager.Instance.AllSkills.Count == 0)
            {
                Console.WriteLine("경고: 스킬 데이터가 로드되지 않았습니다!");
            }
            QuestManager.Instance.LoadQuestDatabase();


            // 임시 아이템 추가(테스트용)
            Item item;
            Player.Inventory.AddItem(item = AllItems.FirstOrDefault(i => i.Id == 0));

            // Scene 등록
            // 작성법 :  scenes[SceneID.씬이름] = new 씬클래스이름(this);
            scenes[GameState.DungeonScene] = new DungeonScene();
            scenes[GameState.InventoryScene] = new InventoryScene();
            scenes[GameState.TownScene] = new TownScene();
            scenes[GameState.StoreScene] = new StoreScene();
            scenes[GameState.QuestScene] = new QuestScene();


            // Monster등록
            monsType[MonsterType.Minion] = new Minion();
            monsType[MonsterType.SigeMinion] = new SiegeMinion();
            monsType[MonsterType.Voidgrub] = new Voidgrub();

            // 초기 Scene 설정
            currentScene = GameState.TownScene;

            // 초기 던전 상태 설정
            _currentState = DungeonState.SelectStart;

            // 초기 던전 난이도 설정
            _currentLevel = DungeonLevel.Normal;

        }

        // 아이템 정보 불러오기
        private void LoadItemDatabase()
        {
            try
            {
                string projectRootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".."));
                string itemDbPath = Path.Combine(projectRootPath, "Json", "items.json");

                if (File.Exists(itemDbPath))
                {
                    string json = File.ReadAllText(itemDbPath, Encoding.UTF8);
                    this.AllItems = JsonConvert.DeserializeObject<List<Item>>(json);
                }
                else
                {
                    this.AllItems = new List<Item>();
                    Console.WriteLine("아이템 데이터베이스 파일을 찾을 수 없습니다!");
                }
            }
            catch (Exception ex)
            {
                // JSON 형식 오류 등 예외 발생 시 처리
                Console.WriteLine($"아이템 데이터베이스 로딩 중 오류 발생: {ex.Message}");
                this.AllItems = new List<Item>();
            }
        }

        private void RenderMenu()
        {
            Console.Clear(); // 화면을 초기화
            scenes[currentScene].RenderMenu();
        }
        private void UpdateInput()
        {
            scenes[currentScene].UpdateInput();
        }
        // ==== Scene 전환 메서드 ====
        public void SwitchScene(GameState id) => currentScene = id;
        public void SwitchState(DungeonState id) => _currentState = id; // 던전 상태 전환 메서드

        // 게임 저장하기 기능
        public void SaveGame()
        {
            // SaveManager SaveGame()에 저장할 플레이어 객체를 보내준다. 
            SaveManager.Instance.SaveGame(this.Player);
        }
        //===================[정진규 추가]

        public void LoadGame()
        {
            SaveData saveData = SaveManager.Instance.LoadGame();
            if (saveData == null) return;

            // Player 객체 정보 불러오기
            // 플레이어가 직접 자신의 정보를 불러오는 것이 캡슐화에 좋다. (즉 다른 정보들도 있다면 해당 클래스에서 스스로 정보를 불러오는 것이 좋음)
            this.Player.LoadFromData(saveData);

            // Player 객체가 가진 완료한 퀘스트 id를 지우고 저장한 데이터를 넣어준다.
            this.Player.CompletedQuestIds.Clear();
            this.Player.CompletedQuestIds.AddRange(saveData.CompletedQuestIds);

            // Player 객체가 가진 인벤토리를 비우고 저장한 보유 아이템 데이터를 넣어준다.
            this.Player.Inventory.Clear();

            // 인벤토리를 불러온다.
            foreach (int itemId in saveData.InventoryItemIds)
            {
                Item item = AllItems.FirstOrDefault(i => i.Id == itemId);
                if (item != null)
                {
                    this.Player.Inventory.AddItem(item.Clone());
                }
            }

            // 장착 아이템을 불러온다.
            if (saveData.EquippedWeaponId != -1)
            {
                Item weapon = this.Player.Inventory.Items.FirstOrDefault(i => i.Id == saveData.EquippedWeaponId);
                if (weapon != null) this.Player.EquipItem(weapon);
            }
            if (saveData.EquippedArmorId != -1)
            {
                Item armor = this.Player.Inventory.Items.FirstOrDefault(i => i.Id == saveData.EquippedArmorId);
                if (armor != null) this.Player.EquipItem(armor);
            }

            // 스킬을 불러온다.
            this.Player.Skills.Clear(); // 기존 스킬을 모두 비웁니다.
            if (saveData.LearnedSkillIds != null)
            {
                foreach (int skillId in saveData.LearnedSkillIds)
                {
                    // SkillManager의 전체 스킬 목록에서 '원본'을 찾습니다.
                    Skill originalSkill = SkillManager.Instance.AllSkills.FirstOrDefault(s => s.Id == skillId);
                    if (originalSkill != null)
                    {
                        // 원본을 복사하여 플레이어에게 지급합니다.
                        this.Player.Skills.Add(originalSkill.Clone());
                    }
                }
            }
        }

        // ==== 게임종료 문구출력 ====
        public void GameOver(string text = "")
        {
            Console.Clear();
            Console.WriteLine($"\n{text}");
            running = false;
        }
    }
}
