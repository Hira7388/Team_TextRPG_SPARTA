using ConsoleTextRPG.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.TurnBasedSystem
{
    public abstract class BaseState
    {
        public static BaseState[] states { get; private set; } = null!;
        public static void Init()
        {
            states = new BaseState[(int)TurnState.Size];
            states[(int)TurnState.Idle] = new IdelState();
            states[(int)TurnState.Battle] = new BattleState();
            states[(int)TurnState.EndBattle] = new EndBattleState();
        }


        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
