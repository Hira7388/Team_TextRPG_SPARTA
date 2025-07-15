using ConsoleTextRPG.Scene;
using ConsoleTextRPG.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleTextRPG.Scenes;

namespace ConsoleTextRPG.Managers
{
    // 게임 Scene 전환을 위한 SceneID 열거형
    // 작성자 : 이영신
    // 아래 열거형에 씬 이름을 추가후 아래 'Init' 함수에서 씬을 등록해주세요.


    internal class GameManager
    {
        // 현재 씬
        private GameState currentScene;
        private readonly Dictionary<GameState, BaseScene> scenes = new();

        // 플레이어 객체 생성
        public Player Player { get; private set; }

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

        //===================[이영신 추가]
        private bool running = true;
        public void GameRun()
        {
            Init();
            if (File.Exists(SaveManager.Instance.SaveFilePath)) // 저장 파일이 있는지 확인
            {
                Console.WriteLine("저장된 게임이 있습니다. 불러오시겠습니까? (Y/N)");
                Console.Write(">> ");
                string input = Console.ReadLine();
                if (input.Trim().ToUpper() == "Y")
                {
                    LoadGame();
                }
            }

            while (running)
            {
                RenderMenu();
                UpdateInput();
            }
        }
        private void Init()
        {
            this.Player = new Player("");
            Console.CursorVisible = false;
            // 임시 아이템 추가(테스트용)
            Player.Inventory.AddItem(new Item(0, "낡은 검", Item.ItemType.Weapon, 5, "쉽게 볼 수 있는 검입니다.", 100));
            // Scene 등록
            // 작성법 :  scenes[SceneID.씬이름] = new 씬클래스이름(this);
            scenes[GameState.DungeonScene] = new DungeonScene();
            scenes[GameState.InventoryScene] = new InventoryScene();
            scenes[GameState.TownScene] = new TownScene();
            scenes[GameState.StoreScene] = new StoreScene();

            // 초기 Scene 설정
            currentScene = GameState.TownScene;
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
        //===================[이영신 추가]

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

            // TODO: 인벤토리 및 장비 복원 로직 추가
            // 이 부분은 Player 클래스 또는 GameManager에서 처리할 수 있다.
            // 예를 들어, 모든 아이템 목록을 가진 GameManager가 ID를 기반으로 아이템을 찾아 플레이어에게 줍니다.
        }
    }
}
