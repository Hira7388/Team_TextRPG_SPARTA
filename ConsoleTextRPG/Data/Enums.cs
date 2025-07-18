using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    internal class Enums
    {
        enum GameState
        {
            TownScene, // 타운(마을) 씬
            StoreScene, // 상점 씬
            InventoryScene, // 인벤토리 씬
            DungeonOutScene, // 던전 밖 씬
            DungeonInScene, // 던전 안 씬
            ViewStatueScene  // 스텟창 씬
        }
    }
<<<<<<< Updated upstream
=======

    // 게임의 상태를 나타내는 열거형
    public enum DungeonState
    {
        SelectStart,
        SelectLevel,
        Adventure,
        PlayerTrun,
        PlayerAttack,
        PlayerSkill,     // 새로 추가
        EnemyTurn,
        EndBattle,
    }

    // 던전 난이도 열거형
    public enum DungeonLevel
    {
        Easy,
        Normal,
        Hard,
    }

    // 몬스터 종류 열거형
    public enum MonsterType
    {
        Minion,
        SigeMinion,
        Voidgrub,
    }


>>>>>>> Stashed changes
}
