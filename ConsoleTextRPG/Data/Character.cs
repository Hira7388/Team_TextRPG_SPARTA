using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    public abstract class Character
    {
        //정승우 캐릭터 수정
        public string Name { get; protected set; }
        public CharacterStat Stat { get; protected set; }

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
                Thread.Sleep(250);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"{this.Name}의 공격!");
                Thread.Sleep(250);
            }

            // 🔸 체력 변화 계산용 이전 체력 저장
            int prevHp = target.Stat.CurrentHp;

            // 데미지 처리 (실제 피해만 적용, 출력 없음)
            target.TakeDamageWithoutPrint(finalDamage);

            // 🔸 방어 중인 몬스터에 대한 메시지 출력
            if (target is Monster monster && monster.IsDefending)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{monster.Name}은(는) 방어 중입니다! 피해가 절반으로 감소합니다.");
                Console.ResetColor();
            }
            // 🔸 실제 입힌 데미지 계산
            int actualDamage = prevHp - target.Stat.CurrentHp;
            if (actualDamage < 0) actualDamage = 0;

            // 🔸 피해량 출력
            Console.WriteLine($"{target.Name}은(는) {actualDamage}의 데미지를 받았습니다.");
            Console.WriteLine($"(기존체력 {prevHp} => 남은 체력: {target.Stat.CurrentHp})");
            Thread.Sleep(250);

            if (target.Stat.IsDead)
            {
                Console.WriteLine($"{target.Name}이(가) 쓰러졌습니다.");
                Thread.Sleep(250);
            }
        }


        // TakeDamageWithoutPrint 함수 (출력 없이 데미지만 적용)
        public virtual bool TakeDamageWithoutPrint(int damage)
        {
            int beforeHp = Stat.CurrentHp;

            if (this is Monster monster && monster.IsDefending)
            {
                damage /= 2;
                int finalDamage = Stat.ApplyDamage(damage);

                // 메시지 생략된 버전이므로 출력 없음
                return Stat.IsDead;
            }
            else
            {
                int finalDamage = Stat.ApplyDamage(damage);
                return Stat.IsDead;
            }
        }




        // 
        public virtual void Defend(Character attacker)
        {
            int damage = attacker.Stat.TotalAttack;
            int finalDamage = TakeDefendDamage(damage);

            //Console.WriteLine($"{this.Name}는 {attacker.Name}의 공격을 받았지만 방어했습니다!!");
            //Console.WriteLine($"원래 데미지: {damage}, 방어력: {this.Stat.TotalDefense * 2}, 감소된 데미지: {finalDamage} (남은 체력: {Stat.CurrentHp})");
            //Thread.Sleep(250);

            // 상태 변경 신호
            GameManager.Instance.currentState = DungeonState.PlayerTurn;
        }


        // target이 damage를 받는 행동
        public virtual bool TakeDamage(int damage)
        {
            int beforeHp = Stat.CurrentHp;
            int originalDamage = damage;

            if (this is Monster monster && monster.IsDefending)
            {
                // 방어 중일 때 데미지 절반 처리
                damage /= 2;
                int finalDamage = Stat.ApplyDamage(damage);

                //Console.WriteLine($"<감소된 데미지: {finalDamage}>");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{monster.Name}은(는) 방어 중입니다! 피해가 절반으로 감소합니다.");
                Console.ResetColor();
                Console.WriteLine($"{monster.Name}은(는) 공격을 받아 {finalDamage}의 데미지를 입었습니다.");
                Console.WriteLine($"(기존체력: {beforeHp} => 남은 체력: {Stat.CurrentHp})");
                Thread.Sleep(250);
                return Stat.IsDead;
            }
            else
            {
                // 방어 중이 아닐 때는 상세 메시지 없이 간단하게만 출력하거나  
                // 필요하면 아래처럼 아주 간단히 메시지 출력
                int final = Stat.ApplyDamage(damage);
                Console.WriteLine($"{this.Name}은(는) {final}의 데미지를 받았습니다. (남은 체력: {Stat.CurrentHp})");
                Thread.Sleep(250);
                return Stat.IsDead;
            }
        }





        // 몬스터가 Player에게 damage를 주는 행동
        public virtual bool MosTakeDamage(int damage)
        {
            int finalDamage = Stat.MosApplyDamage(damage, Stat.Dexterity);
            if (finalDamage == 0)
            {
                Console.WriteLine($"{this.Name}은(는) 공격을 회피했습니다!");
                Thread.Sleep(500);
                return false;
            }
            return Stat.IsDead;
        }




        // 자신이 damage를 받는 행동
        public virtual int TakeDefendDamage(int damage)
        {
            // 실제 데미지 계산 및 적용은 Stat 전문가에게 위임
            int originalDamage = damage;

            // 방어력 적용 후 데미지 계산
            int finalDamage = damage - (this.Stat.TotalDefense * 2);
            if (finalDamage < 0) finalDamage = 0;

            // 데미지 적용 (체력 차감)
            this.Stat.CurrentHp -= finalDamage;
            if (this.Stat.CurrentHp < 0) this.Stat.CurrentHp = 0;

            // 출력: 방어 성공 메시지 + 데미지 감소 내용
            //Console.WriteLine($"{this.Name}은(는) 방어에 성공했습니다!");
            Console.WriteLine($"<원래 데미지: {originalDamage}, 방어력: {this.Stat.TotalDefense * 2}, 받는 데미지: {finalDamage}>");
            Thread.Sleep(250);

            // 사망 여부 체크
            if (Stat.IsDead)
            {
                Console.WriteLine($"{this.Name}이(가) 쓰러졌습니다.");
                Thread.Sleep(250);
            }

            return finalDamage; // ✅ 이 한 줄 추가로 외부에서도 데미지 값 사용 가능
        }
    }
}
