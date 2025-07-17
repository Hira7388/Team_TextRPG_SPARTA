using ConsoleTextRPG.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Monsters
{
    public class Voidgrub : Monster
    {
        public Voidgrub()
        {
            // 몬스터 정보 설정
            Name = "공허충";
            MaxHP = 10;
            Level = 3;
            ATK = 9;
            DFP = 0;
            Gold = 20;
            // 전투 상호작용을 위한 스탯 초기화 설정
            this.Stat = new CharacterStat(Level, ATK, DFP, MaxHP);
        }

    }
}
