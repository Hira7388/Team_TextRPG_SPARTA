using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleTextRPG.Managers
{
    internal class GameManager
    { 
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
<<<<<<< Updated upstream
=======

        private bool running = true;

        public void GameRun()
        {
            Init();
<<<<<<< Updated upstream
            if (File.Exists(SaveManager.Instance.SaveFilePath)) // 저장 파일이 있는지 확인
            {
                Console.WriteLine("저장된 게임이 있습니다. 불러오시겠습니까? (Y/N)");
                Console.Write(">> ");
                string input = Console.ReadLine();
                if (input.Trim().ToUpper() == "Y")
                {
                    LoadGame();
=======

            // 저장된 파일 중 하나라도 있으면 불러오기 메뉴 띄우기
            bool anySaveExists = SaveManager.Instance.SaveFilePaths.Any(path => File.Exists(path));

            if (anySaveExists)
            {
                int? slotToLoad = ShowLoadMenu();
                if (slotToLoad.HasValue)
                {
                    Console.WriteLine("정말 저장된 게임을 불러오시겠습니까?");
                    Console.WriteLine("1. 불러오기");
                    Console.WriteLine("2. 취소");
                    Console.Write(">> ");
                    string confirm = Console.ReadLine()?.Trim();

                    if (confirm == "1")
                    {
                        LoadGame(slotToLoad.Value);
                    }
                    else
                    {
                        Console.WriteLine("새 게임을 시작합니다.\n");
                        // 새 게임 시작 (이름 입력 등은 TownScene에서)
                    }
>>>>>>> Stashed changes
                }
                else
                {
                    Console.WriteLine("새 게임을 시작합니다.\n");
                    // 새 게임 시작
                }
            }

            while (running)
            {
                RenderMenu();
                UpdateInput();
            }
        }

<<<<<<< Updated upstream
        //스킬 클래스
        public static class SkillLoader
        {
            public static Dictionary<string, List<SkillManager>> LoadSkillsByClass(string filePath)
            {
                string json = File.ReadAllText(filePath);
                var skillDict = JsonConvert.DeserializeObject<Dictionary<string, List<SkillManager>>>(json);
                return skillDict ?? new Dictionary<string, List<SkillManager>>();
=======
        private void LoadGame(int slotIndex)
        {
            SaveData saveData = SaveManager.Instance.LoadGame(slotIndex);
            if (saveData == null) return;

            this.Player.LoadFromData(saveData);

            this.Player.CompletedQuestIds.Clear();
            this.Player.CompletedQuestIds.AddRange(saveData.CompletedQuestIds);

            this.Player.Inventory.Clear();

            foreach (int itemId in saveData.InventoryItemIds)
            {
                Item item = AllItems.FirstOrDefault(i => i.Id == itemId);
                if (item != null)
                {
                    this.Player.Inventory.AddItem(item.Clone());
                }
            }

            if (saveData.EquippedWeaponId != -1)
            {
                Item weapon = this.Player.Inventory.Items.FirstOrDefault(i => i.Id == saveData.EquippedWeaponId);
                if (weapon != null) this.Player.EquipItem(weapon);
            }
            if (saveData.EquippedArmorId != -1)
            {
                Item armor = this.Player.Inventory.Items.FirstOrDefault(i => i.Id == saveData.EquippedArmorId);
                if (armor != null) this.Player.EquipItem(armor);
>>>>>>> Stashed changes
            }
        }

        private void Init()
        {
            this.Player = new Player("");
            Console.CursorVisible = false;
            LoadItemDatabase();
            QuestManager.Instance.LoadQuestDatabase();
            //BaseState.Init(); // 상태 목록 초기화

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
        }

        // ==== 게임종료 문구출력 ====
        public void GameOver(string text = "")
        {
            Console.Clear();
            Console.WriteLine($"\n{text}");
            running = false;
        }
>>>>>>> Stashed changes
    }
}
