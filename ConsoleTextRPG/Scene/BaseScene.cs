using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Scene
{
    public abstract class BaseScene
    {
        protected GameManager gameManager;

        public BaseScene(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public abstract void Render();
        public abstract void Update();

        
    }
}
