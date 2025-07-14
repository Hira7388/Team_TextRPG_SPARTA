using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Managers
{
    internal class GameManager
    { 
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
    }
}
