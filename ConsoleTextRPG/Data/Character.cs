using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    internal class Character
    {
        public string Name { get; protected set; }
        public CharacterStat Stat { get; protected set; }

        // 호출한 이가 target를 공격하는 행동
        public virtual void Attack(Character target)
        {
            int damage = this.Stat.Attack;
            Console.WriteLine($"{this.Name}의 공격!");
            target.TakeDamage(damage); // 상대방의 TakeDamage 이벤트를 발동시킴
        }

        // target이 damage를 받는 행동
        public virtual void TakeDamage(int damage)
        {
            // 실제 데미지 계산 및 적용은 Stat 전문가에게 위임
            Stat.ApplyDamage(damage);

            // 데미지를 받은 후의 결과(메시지 출력, 사망 확인 등)는 Character가 직접 처리
            Console.WriteLine($"{this.Name}은(는) {damage}의 데미지를 받았습니다. (남은 체력: {Stat.CurrentHp})");

            if (Stat.IsDead)
            {
                Console.WriteLine($"{this.Name}이(가) 쓰러졌습니다.");
            }
        }
    }
}
