using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    public enum GameState
    {
        TownScene, // 타운(마을) 씬
        StoreScene, // 상점 씬
        InventoryScene, // 인벤토리 씬
        DungeonScene, // 던전 씬
        ViewStatueScene  // 스텟창 씬
    }

    // 게임의 상태를 나타내는 열거형
    public enum TurnState
    {
        Idle,
        Battle,
        EndBattle,
        Size,// Size는 현재 배열의 크기를 시각적으로 나타내주기위한 요소임
    }

    // 몬스터 종류 열거형
    public enum MonsterType
    {
        Minion,
        SigeMinion,
        Voidgrub,
    }


}
