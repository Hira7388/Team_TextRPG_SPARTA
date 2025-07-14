using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.TurnBasedSystem
{
    public class TurnBasedFSM : BaseState
    {
        public enum State
        {
            Idle,
            Battle,
            Victory,
            Size,// Size는 현재 배열의 크기를 시각적으로 나타내주기위한 요소임
        }

        public override void Enter()
        {
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
        }

    }
}

