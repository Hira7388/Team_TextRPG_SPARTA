using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    internal class Character
    {
        //정승우 캐릭터 수정수정

<<<<<<< Updated upstream
=======
        // 호출한 이가 target를 공격하는 행동
        // 작성법 : 공격하는객체.Attack(타겟객체);
        // 이후 데미지를 받는 로직을 실행하고(TakeDamage), 받은 데미지와 방어력을 통해 실제 입는 피해량 계산 로직이 실행됨(Stat.ApplyDamage);
        public virtual void Attack(Character target)
        {
            int baseDamage = this.Stat.TotalAttack;

            Random rand = new Random();
            bool isCritical = rand.Next(0, 4) == 0; // 1/4 확률

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

            target.TakeDamage(finalDamage, isCritical); // 치명타 여부 함께 전달
        }


        // 
        public virtual void Defend(Character target)
        {
            int damage = this.Stat.TotalAttack;
            Console.WriteLine($"{this.Name}의 방어!!");
            this.TakeDefendDamage(damage); // 자신의 TakeDefendDamage 이벤트를 발동시킴
        }

        // target이 damage를 받는 행동
        public virtual void TakeDamage(int damage, bool isCritical = false)
        {
            // 실제 데미지 계산 및 적용은 Stat 전문가에게 위임
<<<<<<< Updated upstream
            Stat.ApplyDamage(damage);
            int finalDamage = damage - this.Stat.TotalDefense;
            if (finalDamage < 1) finalDamage = 1; // 최소 1의 데미지는 받도록 보장
                                                  // 데미지를 받은 후의 결과(메시지 출력, 사망 확인 등)는 Character가 직접 처리
=======
            int finalDamage = Stat.ApplyDamage(damage);


            Console.WriteLine($"[Debug] Damage Taken: 캐릭터 이름='{this.Name}' 데미지={finalDamage} 남은 체력={Stat.CurrentHp}");

            // 데미지를 받은 후의 결과(메시지 출력, 사망 확인 등)는 Character가 직접 처리
            Console.WriteLine($"{this.Name}은(는) {finalDamage}의 데미지를 받았습니다. (남은 체력: {Stat.CurrentHp})");
>>>>>>> Stashed changes

            if (isCritical)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{this.Name}은(는) 치명타로 {finalDamage}의 데미지를 받았습니다! (남은 체력: {Stat.CurrentHp})");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"{this.Name}은(는) {finalDamage}의 데미지를 받았습니다. (남은 체력: {Stat.CurrentHp})");
            }

            if (Stat.IsDead)
            {
                Console.WriteLine($"{this.Name}이(가) 쓰러졌습니다.");
            }
        }

        // 자신이 damage를 받는 행동
        public virtual void TakeDefendDamage(int damage)
        {
            // 실제 데미지 계산 및 적용은 Stat 전문가에게 위임
            Stat.DefecnDamage(damage);
            int finalDamage = (this.Stat.TotalDefense * 2) - damage;
            if (finalDamage < 1) this.Stat.CurrentHp += finalDamage; //  데미지가 방어력을 넘었다면 차이만큼 데미지입음

            if (damage >= 0)
            {
                Console.WriteLine($"{this.Name}은(는) 방어에 성공했습니다. (남은 체력: {Stat.CurrentHp})");
            }
            else
            {
                Console.WriteLine($"{this.Name}은(는) {finalDamage}의 데미지를 받았습니다. (남은 체력: {Stat.CurrentHp})");
            }

            if (Stat.IsDead)
            {
                Console.WriteLine($"{this.Name}이(가) 쓰러졌습니다.");
            }
        }
>>>>>>> Stashed changes
    }
}
