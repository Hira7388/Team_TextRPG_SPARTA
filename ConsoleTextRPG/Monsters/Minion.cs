using ConsoleTextRPG.Data;
using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleTextRPG.Monsters
{
    public class Minion : Monster
    {
        public Minion(Monster ms) : base(ms)
        {
            // 몬스터 정보 설정
            Name = "미니언";
            MaxHp = 15;
            Level = 2;
            ATK = 5;
            DFP = 1;
            Gold = 15;

            // 이미지 설정
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("####################");
            sb.AppendLine("#                  #");
            sb.AppendLine("#   (작고 소듕한)   #");
            sb.AppendLine("#  (미니언)  #");
            sb.AppendLine("#                  #");
            sb.AppendLine("####################");
            Image = sb.ToString();
        }
    }
}
