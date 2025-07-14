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

        private bool running = true;

        public void GameRun()
        {
            Init();
            while (running)
            {
                Render();
                Update();
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


            // 초기 Scene 설정
             currentScene = GameState.TownScene;
        }

        private void Render()
        {
            Console.Clear(); // 화면을 초기화
            scenes[currentScene].Render();
        }
        private void Update()
        {
            scenes[currentScene].Update();
        }
       // ==== Scene 전환 메서드 ====
        public void SwitchScene(GameState id) => currentScene = id;
    }
}
