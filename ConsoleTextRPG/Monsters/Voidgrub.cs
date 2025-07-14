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
        public Voidgrub(Monster ms) : base(ms)
        {
            // 몬스터 정보 설정
            Name = "공허충";
            MaxHp = 10;
            Level = 3;
            ATK = 9;
            DFP = 1;
            Gold = 20;

            // 이미지 설정
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("####################");
            sb.AppendLine("#                  #");
            sb.AppendLine("#   (공허충)   #");
            sb.AppendLine("#  (공허공허)  #");
            sb.AppendLine("#                  #");
            sb.AppendLine("####################");
            Image = sb.ToString();
        }
    }
}
