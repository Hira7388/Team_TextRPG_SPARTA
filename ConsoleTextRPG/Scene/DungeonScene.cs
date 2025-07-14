using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Scene
{
    public class DungeonScene : BaseScene
    {
        // 던전 클리어 조건
        int walkCount = 0; // 이동 횟수
        int dungeonClearCount = 15; // 던전 클리어 횟수
        double monsValue = 0.3f; // 몬스터 등장 확률 (30%)

        public DungeonScene(GameManager game) : base(game)
        {
        }

        public override void Render()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}