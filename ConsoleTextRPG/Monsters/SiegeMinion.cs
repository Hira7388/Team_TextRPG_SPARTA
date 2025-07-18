using ConsoleTextRPG.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Monsters
{
    public class SiegeMinion : Monster
    {
        public SiegeMinion()
        {
            // 몬스터 정보 설정
            Name = "대포미니언";
            MaxHP = 25;
            Level = 5;
            ATK = 25;
            DFP = 0;
            Gold = 50;
            DEX = 0;
            // 전투 상호작용을 위한 스탯 초기화 설정
            this.Stat = new CharacterStat(Level, ATK, DFP, MaxHP, DEX);
        }
    }
}
