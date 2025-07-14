using ConsoleTextRPG.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Managers
{
    // 게임 Scene 전환을 위한 SceneID 열거형
    // 작성자 : 이영신
    // 아래 열거형에 씬 이름을 추가후 아래 'Init' 함수에서 씬을 등록해주세요.
    public enum SceneID
    {
        // 작성법 : 씬 이름, 씬이름, 씬이름 
        Main,       // 메인
        Dungeon
    }


    internal class GameManager
    {
        // 현재 씬
        private SceneID currentScene;
        private readonly Dictionary<SceneID, BaseScene> scenes = new();
        
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
            while (running)
            {
                Render();
                Update();
            }
        }
                private void Init()
        {
            Console.CursorVisible = false;

            // Scene 등록
            // 작성법 :  scenes[SceneID.씬이름] = new 씬클래스이름(this);
            scenes[SceneID.Dungeon] = new DungeonScene(this);


            // 초기 Scene 설정
            // currentScene = SceneID.Main;
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
        public void SwitchScene(SceneID id) => currentScene = id;
        //===================[이영신 추가]


    }
}
