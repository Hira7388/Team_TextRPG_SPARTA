using System;
using System.IO;
using Newtonsoft.Json;
using ConsoleTextRPG.Data;
using ConsoleTextRPG.Scenes;
using ConsoleTextRPG.Managers;
using System.Text;

namespace ConsoleTextRPG.Managers
{
    public class SaveManager
    {
        // 세이브 매니저 싱글톤
        private static SaveManager _instance;
        public static SaveManager Instance
        {
            get
            {
                if (_instance == null) _instance = new SaveManager();
                return _instance;
            }
        }

        private readonly string _saveDirectoryPath;
        public string SaveFilePath { get; private set; }

        // 생성자
        private SaveManager() 
        {
            string projectRootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".."));
            _saveDirectoryPath = Path.Combine(projectRootPath, "Json");
            SaveFilePath = Path.Combine(_saveDirectoryPath, "savedata.json");
        }   

        // 현재 게임 저장하기
        public void SaveGame(Player player)
        {
            // 현재 플레이어의 데이터를 saveData 객체에 옮겨 담는다.
            SaveData saveData = new SaveData
            {
                PlayerName = player.Name,
                PlayerJob = player.Job,
                Gold = player.Gold,
                Level = player.Stat.Level,
                BaseAttack = player.Stat.BaseAttack,
                BaseDefense = player.Stat.BaseDefense,
                MaxHp = player.Stat.MaxHp,
                CurrentHp = player.Stat.CurrentHp,
                InventoryItemIds = player.Inventory.Items.Select(item => item.Id).ToList(), // 플레이어 인벤토리 아이템에서 id를 받아서 리스트로 저장한다.
                EquippedWeaponId = player.EquippedWeapon?.Id ?? -1,
                EquippedArmorId = player.EquippedArmor?.Id ?? -1
            };
            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented); // saveData의 내용을 Json문자열로 변환하는 기능
            Directory.CreateDirectory(_saveDirectoryPath); // 혹시 경로에 Json 폴더가 없으면 생성한다.
            File.WriteAllText(SaveFilePath, json, Encoding.UTF8); // _pathSaveFile 경로에 json 파일을 작성한다.
        }

        // 게임 정보 불러오기(GameManager에서)
        public SaveData LoadGame()
        {
            // 저장된 파일이 없으면 null을 반환한다.
            if (!File.Exists(SaveFilePath))
            {
                return null;
            }

            // 파일에서 JSON 문자열을 읽어온다.
            string json = File.ReadAllText(SaveFilePath, Encoding.UTF8);

            // JSON 문자열을 SaveData 객체로 변환합니다.
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

            return saveData; // 게임매니저로 보낸다.
        }
    }
}
