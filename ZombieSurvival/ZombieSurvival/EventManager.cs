using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieSurvival
{
    class EventManager
    {
        Random rand = new Random();

        // ===== 랜덤 이벤트 발생 =====
        public void TriggerRandomEvent(Player player)
        {
            Console.WriteLine("\n🎲 랜덤 이벤트 발생!");

            int roll = rand.Next(0, 100);

            if (roll < 20)
            {
                ZombieAttack(player);
            }
            else if (roll < 40)
            {
                FindSupplies(player);
            }
            else
            {
                NothingHappens();
            }
        }

        // ===== 이벤트 1: 좀비 습격 =====
        void ZombieAttack(Player player)
        {
            Console.WriteLine("☠️ 좀비 무리가 습격해옵니다!");
            int damage = rand.Next(10, 25);
            player.TakeDamage(damage);
            Console.WriteLine($"HP -{damage}");
        }

        // ===== 이벤트 2: 보급품 발견 =====
        void FindSupplies(Player player)
        {
            Console.WriteLine("🎁 버려진 보급품을 발견했습니다!");
            int foodGain = rand.Next(1, 3);
            int ammoGain = rand.Next(1, 10);

            player.Food += foodGain;
            player.Ammo += ammoGain;

            Console.WriteLine($"Food +{foodGain}, Ammo +{ammoGain}");
        }

        // ===== 이벤트 3: 아무 일도 없음 =====
        void NothingHappens()
        {
            Console.WriteLine("… 다행히 아무 일도 일어나지 않았습니다.");
        }
    }
}
