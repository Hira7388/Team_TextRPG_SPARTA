using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    internal class Monster
    {
<<<<<<< Updated upstream
=======
        public string Name;
        public string Image;
        public int MaxHP;
        public int MaxMP;
        public int ATK;
        public int DFP;
        public int Level;
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
            int damage = this.Stat.BaseAttack;
            Console.WriteLine($"{Name}의 공격!");
            target.TakeDamage(damage); // 상대방의 TakeDamage 이벤트를 발동시킴
        }
        public override void TakeDamage(int damage, bool isCritical = false)
        {
            // 실제 데미지 계산 및 적용은 Stat 전문가에게 위임
            Stat.ApplyDamage(damage);
            int finalDamage = damage - this.Stat.BaseDefense;
            if (finalDamage < 1) finalDamage = 1; // 최소 1의 데미지는 받도록 보장
            // 데미지를 받은 후의 결과(메시지 출력, 사망 확인 등)는 Character가 직접 처리
            Console.WriteLine($"{Name}은(는) {finalDamage}의 데미지를 받았습니다. (남은 체력: {Stat.CurrentHp})");

            if (Stat.IsDead)
            {
                Console.WriteLine($"{Name}이(가) 쓰러졌습니다.");
            }
        }
>>>>>>> Stashed changes
    }
}
