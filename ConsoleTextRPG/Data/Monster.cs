using ConsoleTextRPG.Managers;
using ConsoleTextRPG.Monsters;
using ConsoleTextRPG.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    public abstract class Monster : Character
    {
        public string Image;
        public int MaxHP;
        public int ATK;
        public int DFP;
        public int Level;
        public int DEX;
        public int Gold;
        public bool isDead => Stat.CurrentHp <= 0;
        public virtual void PrintMonster( int no, ConsoleColor c1, ConsoleColor c2)
        {
            if (isDead)
            {
                Console.ForegroundColor = c2;
                Console.WriteLine($" {no}.  Lv.{Level} | {Name} | Dead");
                Console.ResetColor();// 기본 색 복원
            }
            else
            {
                Console.ForegroundColor = c1;   // 번호 색
                Console.Write($" {no}. ");
                Console.ResetColor();// 기본 색 복원
                Console.WriteLine($"Lv.{Level} | {Name} | HP : {this.Stat.CurrentHp}/{MaxHP}");
            }
        }
        public virtual void PrintMonster(ConsoleColor c)
        {
            Console.ForegroundColor = c;
            Console.WriteLine($"Lv.{Level} | {Name} | Dead");
            Console.ResetColor();// 기본 색 복원
        }

        public virtual void PrintMonster()
        {
            Console.WriteLine($"Lv.{Level} | {Name} | HP : {this.Stat.CurrentHp}/{MaxHP}");
        }

        public override void Attack(Character target)
        {
            int baseDamage = this.Stat.TotalAttack;
            Random rand = new Random();
            bool isCritical = rand.Next(0, 20) == 0;
            int finalDamage = baseDamage;

            if (isCritical)
            {
                finalDamage *= 5;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{this.Name}의 치명타 공격!! 데미지 5배!!");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"{this.Name}의 공격!");
            }
            Thread.Sleep(250);

            // ⬇️ 데미지 주고 죽었는지 판단
            bool isDead = target.MosTakeDamage(finalDamage);

            if (isDead)
            {
                Console.WriteLine($"{target.Name}이(가) 쓰러졌습니다.");
                Thread.Sleep(250);
            }
        }


    }

}
