using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    public abstract class Monster : Character
    {
        // 몬스터 목록을 관리하는 정적 리스트
        public static List<Monster> monsterlist { get; private set; } = null!;
        public static void Init()
        {
            // 몬스터 목록 초기화
            monsterlist.Clear();
            monsterlist = new List<Monster>();
        }

        // 몬스터 생성자
        protected Monster monster;
        protected Monster(Monster ms) => monster = ms;

        public string Name;
        public string Image;
        public int CurHp;
        public int MaxHp;
        public int ATK;
        public int DFP;
        public int Level;
        public int Gold;
        public bool IsDead =false;
    }
}
