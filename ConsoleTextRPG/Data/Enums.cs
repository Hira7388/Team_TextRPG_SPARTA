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
}
