using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieSurvival
{
    class Player
    {
        // ===== 상태 변수 =====
        public int Hp;
        public int MaxHp;
        public int Food;
        public int Ammo;
        public int Day;
        public int ActionCount;

        // ===== 생성자 (초기값 설정) =====
        public Player()
        {
            MaxHp = 100;
            Hp = 100;
            Food = 3;
            Ammo = 30;
            Day = 1;
            ActionCount = 0;
        }

        // ===== 생존 여부 =====
        public bool IsAlive()
        {
            return Hp > 0;
        }

        // ===== 데미지 처리 =====
        public void TakeDamage(int damage)
        {
            Hp -= damage;
            if (Hp < 0)
                Hp = 0;
        }

        // ===== 회복 =====
        public void Heal(int amount)
        {
            Hp += amount;
            if (Hp > MaxHp)
                Hp = MaxHp;
        }

        // ===== 식량 소비 =====
        public void ConsumeFood()
        {
            if (Food > 0)
                Food--;
            Console.WriteLine("🍞 음식 하나를 먹었다.");
        }
    }
}
