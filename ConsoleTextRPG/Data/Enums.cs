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
}
